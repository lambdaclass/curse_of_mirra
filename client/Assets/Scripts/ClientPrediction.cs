using System;
using System.Collections.Generic;
using UnityEngine;

public class ClientPrediction
{
    public bool didFirstMovement = false;

    public struct PlayerInput
    {
        public float joystick_x_value;
        public float joystick_y_value;
        public long timestampId;
        public long startTimestamp;
        public long endTimestamp;
        public long serverTimestamp;
    }

    public List<PlayerInput> pendingPlayerInputs = new List<PlayerInput>();

    public Position startingPosition;
    public float lastXSent = 0;
    public float lastYSent = 0;

    public void EnqueuePlayerInput(PlayerInput PlayerInput)
    {
        // finalize last pending input
        PlayerInput lastPlayerInput;
        if (pendingPlayerInputs.Count > 0)
        {
            lastPlayerInput = pendingPlayerInputs[pendingPlayerInputs.Count - 1];
            lastPlayerInput.endTimestamp = PlayerInput.startTimestamp;
            pendingPlayerInputs[pendingPlayerInputs.Count - 1] = lastPlayerInput;
        }
        if (didFirstMovement)
        {
            // add the new input to the list
            pendingPlayerInputs.Add(PlayerInput);
            lastXSent = PlayerInput.joystick_x_value;
            lastYSent = PlayerInput.joystick_y_value;
        }
    }

    public void StopMovement()
    {
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        PlayerInput playerInput = new PlayerInput
        {
            joystick_x_value = 0,
            joystick_y_value = 0,
            timestampId = timestamp,
            startTimestamp = timestamp,
            endTimestamp = 0,
        };

        this.didFirstMovement = true;
        EnqueuePlayerInput(playerInput);
    }

    public void SimulatePlayerState(Entity player, long timestampId, long serverTimestamp)
    {
        UpdateLastAcknowledgedInput(player, timestampId, serverTimestamp);
        RemoveServerAcknowledgedInputs(timestampId);
        SimulatePlayerMovement(player);
    }

    void UpdateLastAcknowledgedInput(Entity player, long timestampId, long serverTimestamp)
    {
        startingPosition = player.Position;
        for (int i = 0; i < pendingPlayerInputs.Count; i++)
        {
            PlayerInput input = pendingPlayerInputs[i];
            if (input.timestampId == timestampId)
            {
                if (input.serverTimestamp != 0)
                {
                    input.startTimestamp += serverTimestamp - input.serverTimestamp;
                }
                input.serverTimestamp = serverTimestamp;
                pendingPlayerInputs[i] = input;
            }
        }
    }

    void RemoveServerAcknowledgedInputs(long timestampId)
    {
        pendingPlayerInputs.RemoveAll((input) => input.timestampId < timestampId);
    }

    void SimulatePlayerMovement(Entity player)
    {
        var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var characterSpeed = player.Speed;

        Debug.Log(characterSpeed);

        Position currentPosition = startingPosition;
        Direction currentDirection = player.Direction;

        pendingPlayerInputs.ForEach(input =>
        {
            long endTimestamp = (input.endTimestamp == 0) ? now : input.endTimestamp;
            float deltaTime = endTimestamp - input.startTimestamp;

            Vector2 movementDirection = new Vector2(input.joystick_x_value, input.joystick_y_value);

            if (movementDirection.x != 0 || movementDirection.y != 0)
            {
                currentDirection = new Direction
                {
                    X = movementDirection.x,
                    Y = movementDirection.y
                };
            }

            movementDirection.Normalize();
            Vector2 movementVector = movementDirection * characterSpeed * deltaTime;

            float positionX = currentPosition.X + (float)(movementVector.x);
            float positionY = currentPosition.Y + (float)(movementVector.y);

            Vector3 newPosition = new Vector3(positionX, 0, positionY);

            newPosition = ClampIfOutOfMap(newPosition);

            currentPosition = new Position { X = newPosition.x, Y = newPosition.z };
        });

        player.Position = currentPosition;
        player.Direction = currentDirection;
    }

    private Vector3 ClampIfOutOfMap(Vector3 newPosition)
    {
        float mapRadius = 5520; // FIXME: This value should be fetched from the backend. Will be fixed in PR#270 (backend)

        Vector3 mapCenterPosition = new Vector3(0, 0, 0);
        float playerDistanceFromMapCenter = Vector3.Distance(newPosition, mapCenterPosition);

        if (playerDistanceFromMapCenter > mapRadius)
        {
            Vector3 fromOriginToObject = newPosition - mapCenterPosition;
            fromOriginToObject *= mapRadius / playerDistanceFromMapCenter;
            newPosition = mapCenterPosition + fromOriginToObject;
        }

        return newPosition;
    }
}
