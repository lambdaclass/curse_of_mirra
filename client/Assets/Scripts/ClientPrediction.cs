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

    static float lastXSent = 0;
    static float lastYSent = 0;

    public void PutPlayerInput(PlayerInput PlayerInput)
    {
        PlayerInput lastPlayerInput;
        if (pendingPlayerInputs.Count > 0)
        {
            lastPlayerInput = pendingPlayerInputs[pendingPlayerInputs.Count - 1];
            lastPlayerInput.endMovementTimestamp = PlayerInput.startMovementTimestamp;
            pendingPlayerInputs[pendingPlayerInputs.Count - 1] = lastPlayerInput;
        }

        pendingPlayerInputs.Add(PlayerInput);
        SetLastSentDirection(PlayerInput.joystick_x_value, PlayerInput.joystick_y_value);
    }

    public void StopMovement(long timestamp)
    {
        PlayerInput playerInput = new PlayerInput
        {
            joystick_x_value = 0,
            joystick_y_value = 0,
            startMovementTimestamp = timestamp,
            endMovementTimestamp = 0,
            timestampId = timestamp
        };
        PutPlayerInput(playerInput);
    }

    public void SetLastSentDirection(float x, float y)
    {
        lastXSent = x;
        lastYSent = y;
    }

    public (float, float) GetLastSentDirection()
    {
        return (lastXSent, lastYSent);
    }

    public void SimulatePlayerState(Entity player, long playerTimestamp, long serverTimestamp)
    {
        Debug.Log("SimulatePlayerState " + playerTimestamp + " " + serverTimestamp);
        UpdateAcknowledgedAction(player, playerTimestamp, serverTimestamp);
        RemoveServerAcknowledgedInputs(player, playerTimestamp);
        SimulatePlayerMovement(player);
    }

    void UpdateAcknowledgedAction(Entity player, long playerTimestamp, long serverTimestamp)
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
                acknowledgedPlayerInput.playerPosition = player.Position;
                input.startMovementTimestamp +=
                    serverTimestamp - acknowledgedPlayerInput.lastTimestamp;
                pendingPlayerInputs[i] = input;
            }
            acknowledgedPlayerInput.lastTimestamp = serverTimestamp;
        }
    }

    void RemoveServerAcknowledgedInputs(Entity player, long playerTimestamp)
    {
        pendingPlayerInputs.RemoveAll(
            (input) =>
                input.endMovementTimestamp != 0
                && input.timestampId < acknowledgedPlayerInput.timestampId
        );
    }

    void SimulatePlayerMovement(Entity player)
    {
        var tickRate = GameServerConnectionManager.Instance.serverTickRate_ms;
        var characterSpeed = player.Speed / 100f;
        Position acknowledgedPosition = acknowledgedPlayerInput.playerPosition;
        if (acknowledgedPlayerInput.timestampId == 0)
        {
            acknowledgedPosition = player.Position;
        }

        long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        pendingPlayerInputs.ForEach(input =>
        {
            Vector2 movementDirection = new Vector2(
                -input.joystick_y_value,
                input.joystick_x_value
            );

            movementDirection.Normalize();

            long endTimestamp =
                (input.endMovementTimestamp == 0) ? now : input.endMovementTimestamp;
            float ticks = (endTimestamp - input.startMovementTimestamp) / tickRate;

            Vector2 movementVector = movementDirection * characterSpeed * ticks;

            Position newPlayerPosition = new Position();

            float newPositionX = acknowledgedPosition.X + movementVector.x;
            float newPositionY = acknowledgedPosition.Y + movementVector.y;

            newPlayerPosition.X = newPositionX;
            newPlayerPosition.Y = newPositionY;

            acknowledgedPosition = newPlayerPosition;
            player.Position = acknowledgedPosition;
        });

        // TODO: fix this block of code
        // var radius = 4900;
        // Position center = new Position() { X = 5000, Y = 5000 };

        // if (Distance_between_positions(player.Position, center) > radius)
        // {
        //     var angle = Angle_between_positions(center, player.Position);

        //     player.Position.X = (ulong)(radius * Math.Cos(angle) + 5000);
        //     player.Position.Y = (ulong)(radius * Math.Sin(angle) + 5000);
        // }
    }

    double Distance_between_positions(Position position_1, Position position_2)
    {
        double p1_x = position_1.X;
        double p1_y = position_1.Y;
        double p2_x = position_2.X;
        double p2_y = position_2.Y;

        double distance_squared = Math.Pow(p1_x - p2_x, 2) + Math.Pow(p1_y - p2_y, 2);

        return Math.Sqrt(distance_squared);
    }

    double Angle_between_positions(Position center, Position target)
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
