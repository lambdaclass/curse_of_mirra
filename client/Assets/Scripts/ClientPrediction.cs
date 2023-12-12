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

    public struct AcknowledgeInputs
    {
        public long timestamp;
        public Position playerPosition;
    }

    public List<PlayerInput> pendingPlayerInputs = new List<PlayerInput>();

    public AcknowledgeInputs acknowledgedPlayerInput = new AcknowledgeInputs(){timestamp = 0, playerPosition = new Position(){X = 0, Y = 0}};

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

    public void simulatePlayerState(Player player, long serverTimestamp, long playerTimestamp)
    {
        updateAcknowledgedAction(player, playerTimestamp);
        removeServerAcknowledgedInputs(player, playerTimestamp);
        
        simulatePlayerMovement(player, serverTimestamp);
    }

    void updateAcknowledgedAction(Player player, long playerTimestamps)
    {
        foreach (var input in pendingPlayerInputs)
        {
            if (input.startMovementTimestamp == playerTimestamps && playerTimestamps != acknowledgedPlayerInput.timestamp)
            {
                acknowledgedPlayerInput.timestamp = playerTimestamps;
                acknowledgedPlayerInput.playerPosition = player.Position;
            }
        }
    }

    void removeServerAcknowledgedInputs(Player player, long playerTimestamp)
    {
        pendingPlayerInputs.RemoveAll(
            (input) =>
                input.endMovementTimestamp != 0 && input.startMovementTimestamp < acknowledgedPlayerInput.timestamp
        );
    }

    void simulatePlayerMovement(Player player, long serverTimestamp)
    {
        // TODO check this
        var characterSpeed = PlayerControls.getBackendCharacterSpeed(player.Id);
        Position algo = acknowledgedPlayerInput.playerPosition;
        if (acknowledgedPlayerInput.timestamp == 0){
            algo = player.Position;
        }
        long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        pendingPlayerInputs.ForEach(input =>
        {
            Vector2 movementDirection = new Vector2(
                -input.joystick_y_value,
                input.joystick_x_value
            );

            movementDirection.Normalize();

            var ticks = 0f;

            if (input.endMovementTimestamp == 0) {
                ticks = (now - input.startMovementTimestamp) / 30;
            } else {
                ticks = (input.endMovementTimestamp - input.startMovementTimestamp) / 30;
            }

            Vector2 movementVector = movementDirection * characterSpeed * ticks;

            Position newPlayerPosition = new Position();

            var newPositionX = (long)algo.X + (long)Math.Round(movementVector.x);
            var newPositionY = (long)algo.Y + (long)Math.Round(movementVector.y);

            newPlayerPosition.X = (ulong)newPositionX;
            newPlayerPosition.Y = (ulong)newPositionY;

            algo = newPlayerPosition;
            player.Position = algo;
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
