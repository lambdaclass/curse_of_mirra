using System;
using System.Collections.Generic;
using UnityEngine;

public class ClientPrediction
{
    private long lastServerTimestamp = 0;
    private long lastTickRate = 30;

    public struct PlayerInput
    {
        public float joystick_x_value;
        public float joystick_y_value;
        public long startMovementTimestamp;
        public long endMovementTimestamp;
        public long timestampId;
    }

    public struct AcknowledgeInputs
    {
        public long timestampId;
        public long lastTimestamp;
        public Position playerPosition;
    }

    public List<PlayerInput> pendingPlayerInputs = new List<PlayerInput>();

    public AcknowledgeInputs acknowledgedPlayerInput = new AcknowledgeInputs()
    {
        timestampId = 0,
        playerPosition = new Position() { X = 0, Y = 0 }
    };

    public void putPlayerInput(PlayerInput PlayerInput)
    {
        PlayerInput lastPlayerInput;
        if (pendingPlayerInputs.Count > 0)
        {
            lastPlayerInput = pendingPlayerInputs[pendingPlayerInputs.Count - 1];
            lastPlayerInput.endMovementTimestamp = PlayerInput.startMovementTimestamp;
            pendingPlayerInputs[pendingPlayerInputs.Count - 1] = lastPlayerInput;
        }

        pendingPlayerInputs.Add(PlayerInput);
    }

    public void simulatePlayerState(Player player, long playerTimestamp, long serverTimestamp)
    {
        updateAcknowledgedAction(player, playerTimestamp, serverTimestamp);
        removeServerAcknowledgedInputs(player, playerTimestamp);
        simulatePlayerMovement(player, serverTimestamp);
    }

    void updateAcknowledgedAction(Player player, long playerTimestamp, long serverTimestamp)
    {
        for (int i = 0; i < pendingPlayerInputs.Count; i++)
        {
            PlayerInput input = pendingPlayerInputs[i];
            if (
                input.startMovementTimestamp == playerTimestamp
                && playerTimestamp != acknowledgedPlayerInput.timestampId
            )
            {
                acknowledgedPlayerInput.timestampId = playerTimestamp;
                acknowledgedPlayerInput.playerPosition = player.Position;
            }
            if (playerTimestamp == acknowledgedPlayerInput.timestampId)
            {
                // Debug.Log($"serverTimestamp: {serverTimestamp}");
                // Debug.Log($"last ackonewledgedInput: {acknowledgedPlayerInput.lastTimestamp}");
                // Debug.Log($"difference: {serverTimestamp - acknowledgedPlayerInput.lastTimestamp}");
                acknowledgedPlayerInput.playerPosition = player.Position;
                input.startMovementTimestamp +=
                    serverTimestamp - acknowledgedPlayerInput.lastTimestamp;
                pendingPlayerInputs[i] = input;
            }
            acknowledgedPlayerInput.lastTimestamp = serverTimestamp;
        }
    }

    void removeServerAcknowledgedInputs(Player player, long playerTimestamp)
    {
        pendingPlayerInputs.RemoveAll(
            (input) =>
                input.endMovementTimestamp != 0
                && input.timestampId < acknowledgedPlayerInput.timestampId
        );
    }

    void simulatePlayerMovement(Player player, long serverTimestamp)
    {
        // TODO check this
        var characterSpeed = PlayerControls.getBackendCharacterSpeed(player.Id);
        Position algo = acknowledgedPlayerInput.playerPosition;
        if (acknowledgedPlayerInput.timestampId == 0)
        {
            algo = player.Position;
        }

        long difference = serverTimestamp - lastServerTimestamp;
        if(serverTimestamp > lastServerTimestamp && difference >= 30 && difference <= 40){
            lastTickRate =  serverTimestamp - lastServerTimestamp;
            lastServerTimestamp = serverTimestamp;
            Debug.Log($"Tickrate is: {lastTickRate}");
        }

        lastTickRate = 30;

        long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        pendingPlayerInputs.ForEach(input =>
        {
            Vector2 movementDirection = new Vector2(
                -input.joystick_y_value,
                input.joystick_x_value
            );

            movementDirection.Normalize();

            var ticks = 0f;

            if (input.endMovementTimestamp == 0)
            {
                ticks = (now - input.startMovementTimestamp) / lastTickRate;
                if((now - input.startMovementTimestamp) < 0){
                    Debug.Log($"Is negative? Now: {now} Start: {input.startMovementTimestamp}");
                }
            }
            else
            {
                ticks = (input.endMovementTimestamp - input.startMovementTimestamp) / lastTickRate;
                if((input.endMovementTimestamp - input.startMovementTimestamp) < 0){
                    Debug.Log($"Is negative? End: {input.endMovementTimestamp} Start: {input.startMovementTimestamp}");
                }
            }

            if (ticks < 0)
            {
                Debug.Log($"eto ta mal {ticks}");
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

        // Debug.Log($"CP Player Position is: ({player.Position.X};{player.Position.Y})");
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
