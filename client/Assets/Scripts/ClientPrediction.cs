using System;
using System.Collections.Generic;
using UnityEngine;
using Communication.Protobuf;

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
        public OldPosition playerPosition;
    }

    public List<PlayerInput> pendingPlayerInputs = new List<PlayerInput>();

    public AcknowledgeInputs acknowledgedPlayerInput = new AcknowledgeInputs()
    {
        timestampId = 0,
        playerPosition = new OldPosition() { X = 0, Y = 0 }
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

    public void SimulatePlayerState(OldPlayer player, long playerTimestamp, long serverTimestamp)
    {
        UpdateAcknowledgedAction(player, playerTimestamp, serverTimestamp);
        RemoveServerAcknowledgedInputs(player, playerTimestamp);
        SimulatePlayerMovement(player);
    }

    void UpdateAcknowledgedAction(OldPlayer player, long playerTimestamp, long serverTimestamp)
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

    void RemoveServerAcknowledgedInputs(OldPlayer player, long playerTimestamp)
    {
        pendingPlayerInputs.RemoveAll(
            (input) =>
                input.endMovementTimestamp != 0
                && input.timestampId < acknowledgedPlayerInput.timestampId
        );
    }

    void SimulatePlayerMovement(OldPlayer player)
    {
        var tickRate = SocketConnectionManager.Instance.serverTickRate_ms;
        var characterSpeed = PlayerControls.getBackendCharacterSpeed(player.Id) / 100f;
        OldPosition acknowledgedPosition = acknowledgedPlayerInput.playerPosition;
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

            OldPosition newPlayerPosition = new OldPosition();

            var newPositionX = (long)acknowledgedPosition.X + (long)Math.Round(movementVector.x);
            var newPositionY = (long)acknowledgedPosition.Y + (long)Math.Round(movementVector.y);

            newPlayerPosition.X = (ulong)newPositionX;
            newPlayerPosition.Y = (ulong)newPositionY;

            acknowledgedPosition = newPlayerPosition;
            player.Position = acknowledgedPosition;
        });

        // TODO: fix this block of code
        var radius = 4900;
        OldPosition center = new OldPosition() { X = 5000, Y = 5000 };

        if (Distance_between_positions(player.Position, center) > radius)
        {
            var angle = Angle_between_positions(center, player.Position);

            player.Position.X = (ulong)(radius * Math.Cos(angle) + 5000);
            player.Position.Y = (ulong)(radius * Math.Sin(angle) + 5000);
        }
    }

    double Distance_between_positions(OldPosition position_1, OldPosition position_2)
    {
        double p1_x = position_1.X;
        double p1_y = position_1.Y;
        double p2_x = position_2.X;
        double p2_y = position_2.Y;

        double distance_squared = Math.Pow(p1_x - p2_x, 2) + Math.Pow(p1_y - p2_y, 2);

        return Math.Sqrt(distance_squared);
    }

    double Angle_between_positions(OldPosition center, OldPosition target)
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
