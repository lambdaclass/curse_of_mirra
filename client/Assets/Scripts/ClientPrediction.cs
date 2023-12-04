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
        if (pendingPlayerInputs.Count > 0)
        {
            lastPlayerInput = pendingPlayerInputs[pendingPlayerInputs.Count - 1];
            lastPlayerInput.endMovementTimestamp = PlayerInput.startMovementTimestamp;
            pendingPlayerInputs[pendingPlayerInputs.Count - 1] = lastPlayerInput;
        }

        if (PlayerInput.joystick_x_value == 0 && PlayerInput.joystick_y_value == 0)
        {
            pendingPlayerInputs = new List<PlayerInput>();
            Debug.Log("zero");
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
        pendingPlayerInputs.RemoveAll(
            (input) =>
                input.endMovementTimestamp != 0 && input.endMovementTimestamp <= serverTimestamp
        );
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
            var tf = input.endMovementTimestamp == 0 ? now : input.endMovementTimestamp;

            if (t0 < serverTimestamp && serverTimestamp < tf)
            {
                deltaTime = tf - serverTimestamp;
            }
            else
            {
                deltaTime = tf - t0;
            }

            Debug.Log($"DeltaTime {deltaTime}");
            float ticks = deltaTime / 30;
            Debug.Log($"manuDeltaTime {ticks}");
            Vector2 movementVector = movementDirection * characterSpeed * ticks;

            Debug.Log($"position plus: {movementVector}");
            Position newPlayerPosition = new Position();

            var newPositionX = (long)player.Position.X + (long)Math.Round(movementVector.x);
            var newPositionY = (long)player.Position.Y + (long)Math.Round(movementVector.y);

            newPlayerPosition.X = (ulong)newPositionX;
            newPlayerPosition.Y = (ulong)newPositionY;

            player.Position = newPlayerPosition;
        });

        var radius = 4900;
        Position center = new Position() { X = 5000, Y = 5000 };

        if (distance_between_positions(player.Position, center) > radius)
        {
            var angle = angle_between_positions(center, player.Position);

            player.Position.X = (ulong)(radius * Math.Cos(angle) + 5000);
            player.Position.Y = (ulong)(radius * Math.Sin(angle) + 5000);
        }
    }

    double distance_between_positions(Position position_1, Position position_2)
    {
        double p1_x = position_1.X;
        double p1_y = position_1.Y;
        double p2_x = position_2.X;
        double p2_y = position_2.Y;

        double distance_squared = Math.Pow(p1_x - p2_x, 2) + Math.Pow(p1_y - p2_y, 2);

        return Math.Sqrt(distance_squared);
    }

    double angle_between_positions(Position center, Position target)
    {
        double p1_x = center.X;
        double p1_y = center.Y;
        double p2_x = target.X;
        double p2_y = target.Y;

        var x_diff = p2_x - p1_x;
        var y_diff = p2_y - p1_y;
        return Math.Atan2(y_diff, x_diff);
    }
}
