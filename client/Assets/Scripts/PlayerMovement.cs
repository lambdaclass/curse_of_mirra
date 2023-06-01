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
        public long x;
        public long y;
        public int player_id;
        public long health;
        public PlayerAction action;
        public long aoe_x;
        public long aoe_y;
    }

    public enum PlayerAction
    {
        Nothing = 0,
        Attacking = 1,
        AttackingAOE = 2,
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
            GameObject player = SocketConnectionManager.Instance.players[playerUpdate.player_id];

            /*
                Player has a speed of 3 tiles per tick. A tile in unity is 0.3f a distance of 0.3f.
                There are 50 ticks per second. A player's velocity is 50 * 0.3f

                In general, if a player's velocity is n tiles per tick, their unity velocity
                is 50 * (n / 10f)

                The above is the player's velocity's magnitude. Their velocity's direction
                is the direction of deltaX, which we can calculate (assumming we haven't lost socket
                frames, but that's fine).
            */
            float character_speed = 0;

            if (playerUpdate.player_id % 2 == 0) {
                // Muflus
                character_speed = 0.3f;
            } else {
                // Hack
                character_speed = 0.5f;
            }

            // This is tick_rate * character_speed. Once we decouple tick_rate from speed on the backend
            // it'll be changed.
            float tickRate = 1000f / SocketConnectionManager.Instance.serverTickRate_ms;
            float velocity = tickRate * character_speed;

            float xChange = (playerUpdate.x / 10f - 50.0f) - player.transform.position.x;
            float yChange = (playerUpdate.y / 10f + 50.0f) - player.transform.position.z;

            Animator m_Animator = player.GetComponent<Character>().CharacterModel.GetComponent<Animator>();
            CharacterOrientation3D characterOrientation = player.GetComponent<CharacterOrientation3D>();
            characterOrientation.ForcedRotation = true;

            bool walking = false;
            if (Mathf.Abs(xChange) >= 0.2f || Mathf.Abs(yChange) >= 0.2f) {
                Vector3 movementDirection = new Vector3(xChange, 0f, yChange);
                movementDirection.Normalize();

                Vector3 newPosition = player.transform.position + movementDirection * velocity * Time.deltaTime;
                player.transform.position = newPosition;
                characterOrientation.ForcedRotationDirection = movementDirection;

                walking = true;
            }
            m_Animator.SetBool("Walking", walking);

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
            bool isAttackingAOE = playerUpdate.action == PlayerAction.AttackingAOE;
            if (isAttackingAOE){
                print(playerUpdate.aoe_x  / 10f - 50.0f);
                print(playerUpdate.aoe_y  / 10f + 50.0f);
            }

            SocketConnectionManager.Instance.players[playerUpdate.player_id]
                .GetComponent<AttackController>()
                .SwordAttack(isAttacking);
        }
    }

    void UpdatePlayerActions()
    {
        List<Player> gamePlayers = SocketConnectionManager.Instance.gamePlayers;
        for (int i = 0; i < SocketConnectionManager.Instance.players.Count; i++)
        {
            var new_position = gamePlayers[i].Position;
            var aoe_position = gamePlayers[i].AoePosition;
            playerUpdates.Enqueue(
                new PlayerUpdate
                {
                    x = (long)new_position.Y,
                    y = -((long)new_position.X),
                    player_id = i,
                    health = gamePlayers[i].Health,
                    action = (PlayerAction)gamePlayers[i].Action,
                    aoe_x = (long)aoe_position.Y,
                    aoe_y = -((long)aoe_position.X),
                }
            );
            if (gamePlayers[i].Health == 0)
            {
                print(SocketConnectionManager.instance.players[i].name);
                SocketConnectionManager.instance.players[i].SetActive(false);
            }
        }
    }
}
