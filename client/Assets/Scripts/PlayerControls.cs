using UnityEngine;
using System.Collections.Generic;
using System;

public class PlayerControls : MonoBehaviour
{
    // I think this should be an instance method
    // instead of void.
    public static void AttackIfInRange(int PlayerId)
    {
        var clientAction = new ClientAction { Action = Action.AutoAttack, Target = PlayerId };
        SocketConnectionManager.Instance.SendAction(clientAction);
    }

    public static void BasicAttack(Vector3 direction)
    {
        RelativePosition relativePosition = new RelativePosition
        {
            X = (long)(direction.x * 100),
            Y = (long)(direction.z * 100)
        };

        var clientAction = new ClientAction { Action = Action.BasicAttack, Position = relativePosition };
        SocketConnectionManager.Instance.SendAction(clientAction);
    }

    public void SendJoystickValues(float x, float y)
    {
        if (x != 0 || y != 0)
        {
            var valuesToSend = new JoystickValues { X = x, Y = y };
            var clientAction = new ClientAction { Action = Action.MoveWithJoystick, MoveDelta = valuesToSend };
            SocketConnectionManager.Instance.SendAction(clientAction);
            var norm = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));

            float characterSpeed = 0;
            int playerId = SocketConnectionManager.Instance.playerId;


            if (playerId % 3 == 0)
            {
                // Uma
                characterSpeed = 3f;
            }
            else if (playerId % 3 == 1)
            {
                // Muflus
                characterSpeed = 5f;
            }
            else
            {
                // Uma
                characterSpeed = 4f;
            }

            var x_norm = (float) Math.Round(x / norm * characterSpeed);
            var y_norm = (float) Math.Round(y / norm * characterSpeed);

            x_norm = x_norm / 10f;
            y_norm = y_norm / 10f;

            EntityUpdates.PlayerInput playerInput = new EntityUpdates.PlayerInput
            {
                grid_delta_x = x_norm,
                grid_delta_y = y_norm,
                timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            };
            SocketConnectionManager.Instance.entityUpdates.putPlayerInput(playerInput);
        }
    }
    public void SendAction()
    {
        if (Input.GetKey(KeyCode.W))
        {
            SendAction(Action.Move, Direction.Up);
        }
        if (Input.GetKey(KeyCode.A))
        {
            SendAction(Action.Move, Direction.Left);
        }
        if (Input.GetKey(KeyCode.D))
        {
            SendAction(Action.Move, Direction.Right);
        }
        if (Input.GetKey(KeyCode.S))
        {
            SendAction(Action.Move, Direction.Down);
        }
    }

    private static void SendAction(Action action, Direction direction)
    {
        ClientAction clientAction = new ClientAction { Action = action, Direction = direction };
        SocketConnectionManager.Instance.SendAction(clientAction);
    }
}
