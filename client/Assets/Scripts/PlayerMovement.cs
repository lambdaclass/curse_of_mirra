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
    [SerializeField] CustomInputManager InputManager;

    public bool showServerGhost = false;
    public bool useClientPrediction;
    public GameObject serverGhost;
    public Direction nextAttackDirection;
    public bool isAttacking = false;

    void Start()
    {
        float clientActionRate = SocketConnectionManager.Instance.serverTickRate_ms / 1000f;
        InvokeRepeating("SendAction", clientActionRate, clientActionRate);
        useClientPrediction = true;
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
    }

    void UpdatePlayerActions()
    {
        GameEvent gameEvent = SocketConnectionManager.Instance.gameEvent;
        for (int i = 0; i < SocketConnectionManager.Instance.gamePlayers.Count; i++)
        {
            Player serverPlayerUpdate = gameEvent.Players[i];

            if (serverPlayerUpdate.Id == (ulong)SocketConnectionManager.Instance.playerId && useClientPrediction) {
                // Move the ghost BEFORE client prediction kicks in, so it only moves up until
                // the last server update.
                if (serverGhost != null)
                {
                    movePlayer(serverGhost, serverPlayerUpdate);
                }
                SocketConnectionManager.Instance.clientPrediction.simulatePlayerState(serverPlayerUpdate, gameEvent.Timestamp);
            }

            GameObject actualPlayer = Utils.GetPlayer(serverPlayerUpdate.Id);
            movePlayer(actualPlayer, serverPlayerUpdate);

            if (serverPlayerUpdate.Health == 0)
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

    private void movePlayer(GameObject player, Player playerUpdate)
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
        var characterSpeed = PlayerControls.getBackendCharacterSpeed(playerUpdate.Id) / 10f;

         // This is tickRate * characterSpeed. Once we decouple tickRate from speed on the backend
         // it'll be changed.
         float tickRate = 1000f / SocketConnectionManager.Instance.serverTickRate_ms;
         float velocity = tickRate * characterSpeed;

         var frontendPosition = Utils.transformBackendPositionToFrontendPosition(playerUpdate.Position);

        float xChange = frontendPosition.x - player.transform.position.x;
        float yChange = frontendPosition.z - player.transform.position.z;

        Animator mAnimator = player
            .GetComponent<Character>()
            .CharacterModel.GetComponent<Animator>();
        CharacterOrientation3D characterOrientation =
            player.GetComponent<CharacterOrientation3D>();
        characterOrientation.ForcedRotation = true;

        bool walking = false;

        Vector2 movementChange = new Vector2(xChange, yChange);

        if (movementChange.magnitude >= 0.2f)
        {
            Vector3 movementDirection = new Vector3(xChange, 0f, yChange);
            movementDirection.Normalize();
            if (playerUpdate.Action == PlayerAction.Teleporting)
            {
                player.transform.position = frontendPosition;
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
        healthComponent.SetHealth(playerUpdate.Health);

        bool isAttackingAttack = playerUpdate.Action == PlayerAction.Attacking;
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
        bool isAttackingAOE = playerUpdate.Action == PlayerAction.AttackingAoe;
        if (
            isAttackingAOE && (LobbyConnection.Instance.playerId != (playerUpdate.Id + 1))
        )
        {
            // FIXME: add logic
        }

        if (playerUpdate.Id == SocketConnectionManager.Instance.playerId) {
            InputManager.CheckSkillCooldown(UIControls.SkillBasic, playerUpdate.BasicSkillCooldownLeft);
            InputManager.CheckSkillCooldown(UIControls.Skill1, playerUpdate.FirstSkillCooldownLeft);
            InputManager.CheckSkillCooldown(UIControls.Skill2, playerUpdate.SecondSkillCooldownLeft);
            InputManager.CheckSkillCooldown(UIControls.Skill3, playerUpdate.ThirdSkillCooldownLeft);
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
            // serverGhost.GetComponent<CharacterHandleWeapon>().enabled = false;
            // serverGhost.GetComponent<Character>().CharacterModel.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = Texture2D.whiteTexture;
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
