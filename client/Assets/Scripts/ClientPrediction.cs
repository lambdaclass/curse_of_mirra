using System.Collections.Generic;
using UnityEngine;
using System;

public class ClientPrediction
{
    public struct PlayerInput
    {
        public float joystick_x_value;
        public float joystick_y_value;
        public long timestamp;
    }

    public List<PlayerInput> pendingPlayerInputs = new List<PlayerInput>();

    public void putPlayerInput(PlayerInput PlayerInput)
    {
        pendingPlayerInputs.Add(PlayerInput);
    }

    public void simulatePlayerState(Player player, long timestamp)
    {
        removeServerAcknowledgedInputs(player, timestamp);
        simulatePlayerMovement(player);
    }

    void removeServerAcknowledgedInputs(Player player, long timestamp)
    {
        pendingPlayerInputs.RemoveAll((input) => input.timestamp <= timestamp);
    }

    void simulatePlayerMovement(Player player)
    {
        // TODO check this
        var characterSpeed = PlayerControls.getBackendCharacterSpeed(player.Id);

        pendingPlayerInputs.ForEach(input =>
        {
            Vector2 movementDirection = new Vector2(
                -input.joystick_y_value,
                input.joystick_x_value
            );

            movementDirection.Normalize();
            Vector2 movementVector = movementDirection * characterSpeed;

            Position newPlayerPosition = new Position();
            var newPositionX = (float)player.Position.X + (float)Math.Round(movementVector.x);
            var newPositionY = (float)player.Position.Y + (float)Math.Round(movementVector.y);

            newPositionX = Math.Min(newPositionX, (10000 - 1));
            newPositionX = Math.Max(newPositionX, 0);
            newPositionY = Math.Min(newPositionY, (10000 - 1));
            newPositionY = Math.Max(newPositionY, 0);

            newPlayerPosition.X = newPositionX;
            newPlayerPosition.Y = newPositionY;

            player.Position = newPlayerPosition;
        });
    }
}
