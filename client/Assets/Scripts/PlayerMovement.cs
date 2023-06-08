using System;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
using MoreMountains.Tools;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] MMTouchJoystick joystickL;
    public Queue<PlayerUpdate> playerUpdates = new Queue<PlayerUpdate>();
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
    }

    public enum ProyectileStatus
    {
        Active = 0,
        Exploded = 1,
    }

    Vector3 backPositionToFrontPosition(Position position){
        var x = (long)position.Y / 10f - 50.0f;
        var y = (-((long)position.X)) / 10f + 50.0f;
        return new Vector3(x, 1f, y);
    }

    void Start()
    {
        float clientActionRate = SocketConnectionManager.Instance.serverTickRate_ms / 1000f;
        InvokeRepeating("SendAction", clientActionRate, clientActionRate);
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
            ClientAction clientAction = new ClientAction { Action = Action.Attack, Direction = nextAttackDirection };
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
            GameObject player = SocketConnectionManager.Instance.players[playerUpdate.playerId];

            /*
                Player has a speed of 3 tiles per tick. A tile in unity is 0.3f a distance of 0.3f.
                There are 50 ticks per second. A player's velocity is 50 * 0.3f

                In general, if a player's velocity is n tiles per tick, their unity velocity
                is 50 * (n / 10f)

                The above is the player's velocity's magnitude. Their velocity's direction
                is the direction of deltaX, which we can calculate (assumming we haven't lost socket
                frames, but that's fine).
            */
            float characterSpeed = 0;

            if (playerUpdate.playerId % 3 == 0) {
                // Muflus
                characterSpeed = 0.3f;
            } else if (playerUpdate.playerId % 3 == 1) {
                // Hack
                characterSpeed = 0.5f;
            } else {
                // Uma
                characterSpeed = 0.4f;
            }

            // This is tickRate * characterSpeed. Once we decouple tickRate from speed on the backend
            // it'll be changed.
            float tickRate = 1000f / SocketConnectionManager.Instance.serverTickRate_ms;
            float velocity = tickRate * characterSpeed;

            float xChange = playerUpdate.playerPosition.x - player.transform.position.x;
            float yChange = playerUpdate.playerPosition.z - player.transform.position.z;

            Animator mAnimator = player.GetComponent<Character>().CharacterModel.GetComponent<Animator>();
            CharacterOrientation3D characterOrientation = player.GetComponent<CharacterOrientation3D>();
            characterOrientation.ForcedRotation = true;

            bool walking = false;
            if (Mathf.Abs(xChange) >= 0.2f || Mathf.Abs(yChange) >= 0.2f)
            {
                Vector3 movementDirection = new Vector3(xChange, 0f, yChange);
                movementDirection.Normalize();

                Vector3 newPosition = player.transform.position + movementDirection * velocity * Time.deltaTime;
                player.transform.position = newPosition;
                characterOrientation.ForcedRotationDirection = movementDirection;

                walking = true;
            }
            mAnimator.SetBool("Walking", walking);

            Health healthComponent = player.GetComponent<Health>();
            healthComponent.SetHealth(playerUpdate.health);

            bool isAttacking = playerUpdate.action == PlayerAction.Attacking;
            player.GetComponent<AttackController>().SwordAttack(isAttacking);
            if (isAttacking)
            {
                print("attack");
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
            bool isAttackingAOE = playerUpdate.action == PlayerAction.AttackingAOE;
            if (isAttackingAOE && (LobbyConnection.Instance.playerId != (playerUpdate.playerId + 1)))
            {
                player.GetComponent<GenericAoeAttack>().ShowAoeAttack(new Vector2(playerUpdate.aoeCenterPosition.x, playerUpdate.aoeCenterPosition.z));
            }

            SocketConnectionManager.Instance.players[playerUpdate.playerId]
                .GetComponent<AttackController>()
                .SwordAttack(isAttacking);
        }
    }

    void UpdatePlayerActions()
    {
        List<Player> gamePlayers = SocketConnectionManager.Instance.gamePlayers;
        for (int i = 0; i < SocketConnectionManager.Instance.players.Count; i++)
        {
            playerUpdates.Enqueue(
                new PlayerUpdate
                {
                    playerPosition = backPositionToFrontPosition(gamePlayers[i].Position),
                    playerId = i,
                    health = gamePlayers[i].Health,
                    action = (PlayerAction)gamePlayers[i].Action,
                    aoeCenterPosition = backPositionToFrontPosition(gamePlayers[i].AoePosition),
                }
            );
            // if (gamePlayers[i].Health == 0)
            // {
            //     SocketConnectionManager.instance.players[i].SetActive(false);
            // }
        }
    }

    void UpdateProyectileActions()
    {
        Dictionary<int, GameObject> projectiles = SocketConnectionManager.Instance.projectiles;
        List<Projectile> gameProjectiles = SocketConnectionManager.Instance.gameProjectiles;
        GameObject projectile;

        var toDelete = new List<int>();
        foreach (var pr in projectiles) {
            if (!gameProjectiles.Exists(x => (int)x.Id == pr.Key))
            {
                toDelete.Add(pr.Key);
            }
        }

        foreach (var key in toDelete) {
            Destroy(projectiles[key]);
            projectiles.Remove(key);
        }

        var toExplode = new List<int>();
        foreach (var pr in projectiles) {
            if (gameProjectiles.Find(x => (int)x.Id == pr.Key).Status == ProjectileStatus.Exploded)
            {
                toExplode.Add(pr.Key);
            }
        }

        foreach (var key in toExplode) {
            Destroy(projectiles[key]);
            projectiles.Remove(key);
        }

        for (int i = 0; i < gameProjectiles.Count; i++)
        {
            if (projectiles.TryGetValue((int)gameProjectiles[i].Id, out projectile))
            {
                float projectileSpeed = gameProjectiles[i].Speed / 10f;

                float tickRate = 1000f / SocketConnectionManager.Instance.serverTickRate_ms;
                float velocity = tickRate * projectileSpeed;

                Vector3 backToFrontPosition = backPositionToFrontPosition(gameProjectiles[i].Position);
                float xChange = backToFrontPosition.x - projectile.transform.position.x;
                float yChange = backToFrontPosition.z - projectile.transform.position.z;

                Vector3 movementDirection = new Vector3(xChange, 0f, yChange);
                movementDirection.Normalize();

                Vector3 newPosition = projectile.transform.position + movementDirection * velocity * Time.deltaTime;
                projectile.transform.position = new Vector3(newPosition[0], 1f, newPosition[2]);
            }
            else if (gameProjectiles[i].Status == ProjectileStatus.Active)
            {
                projectile = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Destroy(projectile.GetComponent<BoxCollider>());
                projectile.transform.localScale = new Vector3(.5f, .5f, .5f);
                projectile.transform.position = backPositionToFrontPosition(gameProjectiles[i].Position);
                projectiles.Add((int)gameProjectiles[i].Id, projectile);
            }
        }
    }
}
