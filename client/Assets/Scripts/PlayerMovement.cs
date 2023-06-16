using System;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
using MoreMountains.Tools;
using System.Linq;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    MMTouchJoystick joystickL;

    public Queue<EntityUpdates.PlayerState> playerUpdates = new Queue<EntityUpdates.PlayerState>();
    public Queue<EntityUpdates.PlayerState> serverUpdates = new Queue<EntityUpdates.PlayerState>();
    public bool showServerGhost = false;
    public bool useClientPrediction;
    public GameObject serverGhost;
    public Direction nextAttackDirection;
    public bool isAttacking = false;

    public struct PlayerUpdate
    {
        public Vector3 playerPosition;
        public int playerId;
        public long health;
        public PlayerAction action;
        public Vector3 aoeCenterPosition;
    }

    public enum PlayerAction
    {
        Nothing = 0,
        Attacking = 1,
        AttackingAOE = 2,
        MainAttack = 3,
        Teleporting = 4,
    }

    public enum ProyectileStatus
    {
        Active = 0,
        Exploded = 1,
    }

    void Start()
    {
        float clientActionRate = SocketConnectionManager.Instance.serverTickRate_ms / 1000f;
        InvokeRepeating("SendAction", clientActionRate, clientActionRate);
        useClientPrediction = false;
    }

    void Update()
    {
        if (
            SocketConnectionManager.Instance.gamePlayers != null
            && SocketConnectionManager.Instance.players.Count > 0
            && SocketConnectionManager.Instance.gamePlayers.Count > 0
        )
        {
            UpdatePlayerActions();
            checkForAttacks();
            ExecutePlayerAction();
            UpdateProyectileActions();
        }
    }

    public void SendAction()
    {
        var inputFromPhysicalJoystick = Input.GetJoystickNames().Length > 0;
        var inputFromVirtualJoystick = joystickL is not null;
        if (inputFromPhysicalJoystick)
        {
            var hInput = Input.GetAxis("Horizontal");
            var vInput = Input.GetAxis("Vertical");
            GetComponent<PlayerControls>().SendJoystickValues(hInput, -vInput);
        }
        else if (inputFromVirtualJoystick && joystickL.RawValue.x != 0 || joystickL.RawValue.y != 0)
        {
            GetComponent<PlayerControls>().SendJoystickValues(joystickL.RawValue.x, joystickL.RawValue.y);
        }
        else
        {
            GetComponent<PlayerControls>().SendAction();
        }
        sendAttack();
    }

    void sendAttack()
    {
        if (isAttacking)
        {
            ClientAction clientAction = new ClientAction
            {
                Action = Action.Attack,
                Direction = nextAttackDirection
            };
            SocketConnectionManager.Instance.SendAction(clientAction);
            isAttacking = false;
        }
    }

    void checkForAttacks()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            nextAttackDirection = Direction.Down;
            isAttacking = true;
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            nextAttackDirection = Direction.Up;
            isAttacking = true;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            nextAttackDirection = Direction.Right;
            isAttacking = true;
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            nextAttackDirection = Direction.Left;
            isAttacking = true;
        }
        // Hardcoded dual sense square button
        if (Input.GetKeyDown("joystick 1 button 0"))
        {
            nextAttackDirection = Direction.Up;
            isAttacking = true;
        }
    }

    void ExecutePlayerAction()
    {
        while (playerUpdates.TryDequeue(out var playerUpdate))
        {
            GameObject player = Utils.GetPlayer(playerUpdate.playerId);
            movePlayer(player, playerUpdate);
        }

        while (serverUpdates.TryDequeue(out var playerUpdate))
        {
            if (serverGhost != null)
            {
                movePlayer(serverGhost, playerUpdate);
            }
        }
    }

    void UpdatePlayerActions()
    {
        GameEvent gameEvent = SocketConnectionManager.Instance.gameEvent;
        for (int i = 0; i < SocketConnectionManager.Instance.gamePlayers.Count; i++)
        {
            Player player = gameEvent.Players[i];

            EntityUpdates.PlayerState playerState = new EntityUpdates.PlayerState
            {
                playerPosition = Utils.transformBackendPositionToFrontendPosition(player.Position),
                playerId = (int)player.Id,
                health = player.Health,
                action = (EntityUpdates.PlayerState.PlayerAction)player.Action,
                aoeCenterPosition = Utils.transformBackendPositionToFrontendPosition(player.AoePosition),
                timestamp = gameEvent.Timestamp,
            };
            if (useClientPrediction) {
                if (player.Id == (ulong)SocketConnectionManager.Instance.playerId)
                {
                    serverUpdates.Enqueue(playerState);
                    SocketConnectionManager.Instance.entityUpdates.putServerUpdate(playerState);
                }

                if (player.Id == (ulong)SocketConnectionManager.Instance.playerId && !SocketConnectionManager.Instance.entityUpdates.inputsIsEmpty())
                {
                    playerState = SocketConnectionManager.Instance.entityUpdates.simulatePlayerState();
                    playerState.health = player.Health;
                    playerState.action = (EntityUpdates.PlayerState.PlayerAction)player.Action;
                    playerState.aoeCenterPosition = Utils.transformBackendPositionToFrontendPosition(player.AoePosition);
                    playerState.timestamp = gameEvent.Timestamp;
                }
            }            

            playerUpdates.Enqueue(playerState);

            if (playerState.health == 0)
            {
                SocketConnectionManager.Instance.players[i].SetActive(false);
            }
        }
    }

    void UpdateProyectileActions()
    {
        Dictionary<int, GameObject> projectiles = SocketConnectionManager.Instance.projectiles;
        List<Projectile> gameProjectiles = SocketConnectionManager.Instance.gameProjectiles;
        GameObject projectile;

        var toDelete = new List<int>();
        foreach (var pr in projectiles)
        {
            if (!gameProjectiles.Exists(x => (int)x.Id == pr.Key))
            {
                toDelete.Add(pr.Key);
            }
        }

        foreach (var key in toDelete)
        {
            // TODO unbind projectile destroy from player
            GameObject player = SocketConnectionManager.Instance.players[0];
            player.GetComponent<MainAttack>().LaserDisappear(projectiles[key]);
            projectiles.Remove(key);

        }

        var toExplode = new List<int>();
        foreach (var pr in projectiles)
        {
            if (gameProjectiles.Find(x => (int)x.Id == pr.Key).Status == ProjectileStatus.Exploded)
            {
                toExplode.Add(pr.Key);
            }
        }

        foreach (var key in toExplode)
        {
            // TODO unbind projectile destroy from player
            GameObject player = SocketConnectionManager.Instance.players[0];
            player.GetComponent<MainAttack>().LaserCollision(projectiles[key]);
            projectiles.Remove(key);
        }

        for (int i = 0; i < gameProjectiles.Count; i++)
        {
            if (projectiles.TryGetValue((int)gameProjectiles[i].Id, out projectile))
            {
                float projectileSpeed = gameProjectiles[i].Speed / 10f;

                float tickRate = 1000f / SocketConnectionManager.Instance.serverTickRate_ms;
                float velocity = tickRate * projectileSpeed;

                Vector3 backToFrontPosition = Utils.transformBackendPositionToFrontendPosition(
                    gameProjectiles[i].Position
                );
                float xChange = backToFrontPosition.x - projectile.transform.position.x;
                float yChange = backToFrontPosition.z - projectile.transform.position.z;

                Vector3 movementDirection = new Vector3(xChange, 0f, yChange);
                movementDirection.Normalize();

                Vector3 newPosition = projectile.transform.position + movementDirection * velocity * Time.deltaTime;

                GameObject player = SocketConnectionManager.Instance.players[(int)gameProjectiles[i].PlayerId - 1];
                player.GetComponent<MainAttack>().ShootLaser(projectile, new Vector3(newPosition[0], 1f, newPosition[2]));

            }
            else if (gameProjectiles[i].Status == ProjectileStatus.Active)
            {
                float angle = Vector3.SignedAngle(new Vector3(1f, 0, 0),
                new Vector3((long)(gameProjectiles[i].Direction.Y * 100), 0f, -(long)(gameProjectiles[i].Direction.X * 100)),
                Vector3.up);
                GameObject player = SocketConnectionManager.Instance.players[(int)gameProjectiles[i].PlayerId - 1];
                GameObject newProjectile = player.GetComponent<MainAttack>().InstanceShoot(angle);

                projectiles.Add((int)gameProjectiles[i].Id, newProjectile);
            }
        }
    }

    private void movePlayer(GameObject player, EntityUpdates.PlayerState playerUpdate)
    {
        /*
        Player has a speed of 3 tiles per tick. A tile in unity is 0.3f a distance of 0.3f.
        There are 50 ticks per second. A player's velocity is 50 * 0.3f

        In general, if a player's velocity is n tiles per tick, their unity velocity
        is 50 * (n / 10f)

        The above is the player's velocity's magnitude. Their velocity's direction
        is the direction of deltaX, which we can calculate (assumming we haven't lost socket
        frames, but that's fine).
        */
        var characterSpeed = PlayerControls.getCharacterSpeed(playerUpdate.playerId) / 10f;

         // This is tickRate * characterSpeed. Once we decouple tickRate from speed on the backend
         // it'll be changed.
         float tickRate = 1000f / SocketConnectionManager.Instance.serverTickRate_ms;
         float velocity = tickRate * characterSpeed;

        float xChange = playerUpdate.playerPosition.x - player.transform.position.x;
        float yChange = playerUpdate.playerPosition.z - player.transform.position.z;

        Animator mAnimator = player
            .GetComponent<Character>()
            .CharacterModel.GetComponent<Animator>();
        CharacterOrientation3D characterOrientation =
            player.GetComponent<CharacterOrientation3D>();
        characterOrientation.ForcedRotation = true;

        bool walking = false;

        if (Mathf.Abs(xChange) >= 0.2f || Mathf.Abs(yChange) >= 0.2f)
        {
            Vector3 movementDirection = new Vector3(xChange, 0f, yChange);
            movementDirection.Normalize();
            if (playerUpdate.action == EntityUpdates.PlayerState.PlayerAction.Teleporting)
            {
                player.transform.position = playerUpdate.playerPosition;
                if (playerUpdate.playerId == SocketConnectionManager.Instance.playerId) {
                    SocketConnectionManager.Instance.entityUpdates.lastServerUpdate.playerPosition = playerUpdate.playerPosition;
                }
            }
            else
            {
                Vector3 newPosition =
                player.transform.position + movementDirection * velocity * Time.deltaTime;
                player.transform.position = newPosition;
                characterOrientation.ForcedRotationDirection = movementDirection;
                walking = true;
            }
            
        }
        mAnimator.SetBool("Walking", walking);

        Health healthComponent = player.GetComponent<Health>();
        healthComponent.SetHealth(playerUpdate.health);

        bool isAttackingAttack = playerUpdate.action == EntityUpdates.PlayerState.PlayerAction.Attacking;
        player.GetComponent<AttackController>().SwordAttack(isAttackingAttack);
        if (isAttackingAttack)
        {
            print(player.name + "attack");
        }

        //if dead remove the player from the scene
        if (healthComponent.CurrentHealth <= 0)
        {
            healthComponent.Model.gameObject.SetActive(false);
        }
        if (healthComponent.CurrentHealth == 100)
        {
            healthComponent.Model.gameObject.SetActive(true);
        }
        bool isAttackingAOE = playerUpdate.action == EntityUpdates.PlayerState.PlayerAction.AttackingAOE;
        if (
            isAttackingAOE && (LobbyConnection.Instance.playerId != (playerUpdate.playerId + 1))
        )
        {
            // FIXME: add logic
        }
    }

    public void ToggleGhost()
    {
        if (!useClientPrediction) {
            return;
        }
        showServerGhost = !showServerGhost;
        if (showServerGhost)
        {
            GameObject player = Utils.GetPlayer(SocketConnectionManager.Instance.playerId);
            serverGhost = Instantiate(player, player.transform.position, Quaternion.identity);
            serverGhost.GetComponent<Character>().name = "Server Ghost";
            serverGhost.GetComponent<CharacterHandleWeapon>().enabled = false;
            serverGhost.GetComponent<Character>().CharacterModel.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = Texture2D.whiteTexture;
        }
        else
        {
            serverGhost.GetComponent<Character>().GetComponent<Health>().SetHealth(0);
            Destroy(serverGhost);
            serverGhost = null;
        }
    }

    public void ToggleClientPrediction()
    {
        useClientPrediction = !useClientPrediction;
        Text toggleGhostButton = GameObject.Find("ToggleCPText").GetComponent<Text>();
        if (!useClientPrediction) {
            toggleGhostButton.text = "Client Prediction Off";
            showServerGhost = false;
            if (serverGhost != null) {
                serverGhost.GetComponent<Character>().GetComponent<Health>().SetHealth(0);
                Destroy(serverGhost);
            }
        } else {
            toggleGhostButton.text = "Client Prediction On";
        }
    }
}
