using System;
using System.Collections.Generic;
using UnityEngine;

public class ClientPrediction
{
    public struct PlayerInput
    {
        public float joystick_x_value;
        public float joystick_y_value;
        public long timestampId;
        public long startTimestamp;
        public long endTimestamp;
    }

    public struct AcknowledgedInput
    {
        public long timestampId;
        public Position position;
    }

    public List<PlayerInput> pendingPlayerInputs = new List<PlayerInput>();

    public AcknowledgedInput lastAcknowledgedInput = new AcknowledgedInput
    {
        timestampId = 0,
        position = new Position { X = 0, Y = 0 }
    };

    public void putPlayerInput(PlayerInput PlayerInput)
    {
        // finalize last pending input
        PlayerInput lastPlayerInput;
        if (pendingPlayerInputs.Count > 0)
        {
            lastPlayerInput = pendingPlayerInputs[pendingPlayerInputs.Count - 1];
            lastPlayerInput.endTimestamp = PlayerInput.startTimestamp;
            pendingPlayerInputs[pendingPlayerInputs.Count - 1] = lastPlayerInput;
        }
        // add the new one
        pendingPlayerInputs.Add(PlayerInput);
    }

    public void simulatePlayerState(Entity player, long timestamp)
    {
        if (lastAcknowledgedInput.timestampId < timestamp)
        {
            lastAcknowledgedInput.timestampId = timestamp;
            lastAcknowledgedInput.position = player.Position;
        }
        removeServerAcknowledgedInputs(player, timestamp);
        simulatePlayerMovement(player);
    }

    void removeServerAcknowledgedInputs(Entity player, long timestamp)
    {
        pendingPlayerInputs.RemoveAll((input) => input.timestampId < timestamp);
    }

    void simulatePlayerMovement(Entity player)
    {
        var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var characterSpeed = player.Speed;

        Position initialPosition = player.Position;
        if (lastAcknowledgedInput.timestampId != 0)
        {
            initialPosition = lastAcknowledgedInput.position;
        }

        pendingPlayerInputs.ForEach(input =>
        {
            long endTimestamp = (input.endTimestamp == 0) ? now : input.endTimestamp;
            // TODO: remove magic numbers
            float tickRate = (input.endTimestamp == 0) ? 31f : 30f;
            float ticks = (float)Math.Floor((endTimestamp - input.startTimestamp) / tickRate);

            Vector2 movementDirection = new Vector2(input.joystick_x_value, input.joystick_y_value);

            movementDirection.Normalize();
            Vector2 movementVector = movementDirection * characterSpeed * ticks;

            Position newPlayerPosition = new Position
            {
                X = initialPosition.X + (float)(movementVector.x),
                Y = initialPosition.Y + (float)(movementVector.y)
            };
            initialPosition = newPlayerPosition;
            player.Position = initialPosition;
        });
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
