using System;
using System.Collections.Generic;
using UnityEngine;

public class ClientPrediction
{
    public struct PlayerInput
    {
        public float joystick_x_value;
        public float joystick_y_value;
        public long startMovementTimestamp;
        public long endMovementTimestamp;
    }

    public List<PlayerInput> pendingPlayerInputs = new List<PlayerInput>();

    public void putPlayerInput(PlayerInput PlayerInput)
    {
        PlayerInput lastPlayerInput;
        if(pendingPlayerInputs.Count > 0){
            lastPlayerInput = pendingPlayerInputs[pendingPlayerInputs.Count - 1];
            lastPlayerInput.endMovementTimestamp = PlayerInput.startMovementTimestamp;
            pendingPlayerInputs[pendingPlayerInputs.Count - 1] = lastPlayerInput;
        }
        pendingPlayerInputs.Add(PlayerInput);
    }

    public void simulatePlayerState(Player player, long serverTimestamp)
    {
        removeServerAcknowledgedInputs(player, serverTimestamp);
        simulatePlayerMovement(player, serverTimestamp);
    }

    void removeServerAcknowledgedInputs(Player player, long serverTimestamp)
    {
        pendingPlayerInputs.RemoveAll((input) => input.endMovementTimestamp != 0 && input.endMovementTimestamp < serverTimestamp);
    }

    void simulatePlayerMovement(Player player, long serverTimestamp)
    {
        // TODO check this
        var characterSpeed = PlayerControls.getBackendCharacterSpeed(player.Id);
        long deltaTime;
        long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        pendingPlayerInputs.ForEach(input =>
        {
            Vector2 movementDirection = new Vector2(
                -input.joystick_y_value,
                input.joystick_x_value
            );

            movementDirection.Normalize();

            var t0 = input.startMovementTimestamp;
            var tf = input.endMovementTimestamp == 0 ? now: input.endMovementTimestamp;

            if(t0 < serverTimestamp && serverTimestamp < tf ){
                deltaTime = tf - serverTimestamp;
            }else{
                deltaTime = tf - t0;
            }

            Vector2 movementVector = movementDirection * characterSpeed * deltaTime / 30;

            Position newPlayerPosition = new Position();
            var newPositionX = (long)player.Position.X + (long)Math.Round(movementVector.x);
            var newPositionY = (long)player.Position.Y + (long)Math.Round(movementVector.y);

            newPositionX = Math.Min(newPositionX, (10000 - 1));
            newPositionX = Math.Max(newPositionX, 0);
            newPositionY = Math.Min(newPositionY, (10000 - 1));
            newPositionY = Math.Max(newPositionY, 0);

            newPlayerPosition.X = (ulong)newPositionX;
            newPlayerPosition.Y = (ulong)newPositionY;

            player.Position = newPlayerPosition;
        });
    }
}
