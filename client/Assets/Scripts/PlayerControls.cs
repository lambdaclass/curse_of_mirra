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

            int playerId = SocketConnectionManager.Instance.playerId;
            float characterSpeed = getCharacterSpeed(playerId);

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
            int playerId = SocketConnectionManager.Instance.playerId;
            float characterSpeed = getCharacterSpeed(playerId);
            var (delta_x, delta_y) = keyboardMovementDelta(characterSpeed, Direction.Up);


            EntityUpdates.PlayerInput playerInput = new EntityUpdates.PlayerInput
            {
                grid_delta_x = delta_x,
                grid_delta_y = delta_y,
                timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            };
            SocketConnectionManager.Instance.entityUpdates.putPlayerInput(playerInput);
        }
        if (Input.GetKey(KeyCode.A))
        {
            SendAction(Action.Move, Direction.Left);
            int playerId = SocketConnectionManager.Instance.playerId;
            float characterSpeed = getCharacterSpeed(playerId);
            var (delta_x, delta_y) = keyboardMovementDelta(characterSpeed, Direction.Left);

            EntityUpdates.PlayerInput playerInput = new EntityUpdates.PlayerInput
            {
                grid_delta_x = delta_x,
                grid_delta_y = delta_y,
                timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            };
            SocketConnectionManager.Instance.entityUpdates.putPlayerInput(playerInput);
        }
        if (Input.GetKey(KeyCode.D))
        {
            SendAction(Action.Move, Direction.Right);
            int playerId = SocketConnectionManager.Instance.playerId;
            float characterSpeed = getCharacterSpeed(playerId);
            var (delta_x, delta_y) = keyboardMovementDelta(characterSpeed, Direction.Right);

            EntityUpdates.PlayerInput playerInput = new EntityUpdates.PlayerInput
            {
                grid_delta_x = delta_x,
                grid_delta_y = delta_y,
                timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            };
            SocketConnectionManager.Instance.entityUpdates.putPlayerInput(playerInput);
        }
        if (Input.GetKey(KeyCode.S))
        {
            SendAction(Action.Move, Direction.Down);
            int playerId = SocketConnectionManager.Instance.playerId;
            float characterSpeed = getCharacterSpeed(playerId);
            var (delta_x, delta_y) = keyboardMovementDelta(characterSpeed, Direction.Down);

            EntityUpdates.PlayerInput playerInput = new EntityUpdates.PlayerInput
            {
                grid_delta_x = delta_x,
                grid_delta_y = delta_y,
                timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            };
            SocketConnectionManager.Instance.entityUpdates.putPlayerInput(playerInput);
        }
    }

    private (float, float) keyboardMovementDelta(float characterSpeed, Direction direction) {
        var delta = (0f, 0f);
        float speed = characterSpeed / 10f;

        if (direction == Direction.Up) {
            delta = (0f, speed);
        } else if (direction == Direction.Left) {
            delta = (-speed, 0f);
        } else if (direction == Direction.Right) {
            delta = (speed, 0f);
        } else if (direction == Direction.Down) {
            delta = (0f, -speed);
        }

        return delta;
    }
 
    public static float getCharacterSpeed(int playerId) {
        var characterSpeed = 0f;
        if (playerId % 3 == 0)
        {
            // Uma
            characterSpeed = 3f;
        }
        else if (playerId % 3 == 1)
        {
            // Muflus
            characterSpeed = 4f;
        }
        else
        {
            // Uma
            characterSpeed = 5f;
        }

        return characterSpeed;
    }

    private static void SendAction(Action action, Direction direction)
    {
        ClientAction clientAction = new ClientAction { Action = action, Direction = direction };
        SocketConnectionManager.Instance.SendAction(clientAction);
    }
}
