using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement
{
    public struct Movement
    {
        public Position position;
        public Direction direction;
        public float speed;
        public long movementTimestamp;
        public long timestamp;
        public long tick;
    }

    public List<Movement> movements = new List<Movement>();

    public Entity player = new Entity();

    public long lastTimestamp = 0;
    public long currentTimestamp = 0;

    public float mapRadius;

    public long tick = -1;

    public struct ServerInfo
    {
        public Entity player;
        public long timestamp;
    }

    public Dictionary<long, bool> timestampsRead = new Dictionary<long, bool>();

    public ServerInfo gameState;

    public void MovePlayer()
    {
        if (this.tick == -1)
        {
            return;
        }
        long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        long deltaTime = now - lastTimestamp;

        player.Position.X += player.Direction.X * player.Speed * deltaTime;
        player.Position.Y += player.Direction.Y * player.Speed * deltaTime;

        // Refactor this {
        Vector3 newPosition = new Vector3(player.Position.X, 0, player.Position.Y);
        newPosition = ClampIfOutOfMap(newPosition, player.Radius);

        player.Position.X = newPosition.x;
        player.Position.Y = newPosition.z;
        processCollisions();
        // }

        lastTimestamp = now;

        Movement movement = new Movement
        {
            position = player.Position,
            direction = player.Direction,
            speed = player.Speed,
            movementTimestamp = currentTimestamp,
            timestamp = lastTimestamp,
            tick = now - GameServerConnectionManager.Instance.clientTimestampStarted
        };
        movements.Add(movement);
        this.tick += 1;
    }

    public void ReconciliatePlayer(float reconciliationDistance)
    {
        int index = 0;
        while (
            movements[index].movementTimestamp < gameState.timestamp && index < movements.Count - 1
        )
        {
            index += 1;
        }

        if (timestampsRead.ContainsKey(movements[index].movementTimestamp))
        {
            return;
        }

        timestampsRead[movements[index].movementTimestamp] = true;

        float distance = Vector3.Distance(
            new Vector3(movements[index].position.X, 0, movements[index].position.Y),
            new Vector3(gameState.player.Position.X, 0, gameState.player.Position.Y)
        );
        if (distance > reconciliationDistance)
        {
            Debug.Log("=== Reconciliating player ===");
            player.Position = gameState.player.Position;
            for (int i = index + 1; i < movements.Count; i++)
            {
                Movement movement = movements[i];
                long now = lastTimestamp;
                if (i < movements.Count - 1)
                {
                    now = movements[i + 1].timestamp;
                }
                long deltaTime = now - movement.timestamp;
                // Debug.Log(deltaTime);
                player.Position.X += movement.direction.X * movement.speed * deltaTime;
                player.Position.Y += movement.direction.Y * movement.speed * deltaTime;
            }
            // Debug.Log("=== END Reconciliating player ===");
        }
    }

    public void AddMovement(Direction direction, long timestamp)
    {
        if (player.Player.ForcedMovement)
        {
            return;
        }
        player.Direction.X = direction.X;
        player.Direction.Y = direction.Y;
        currentTimestamp = timestamp;
    }

    public void AddForcedMovement(Direction direction)
    {
        player.Direction.X = direction.X;
        player.Direction.Y = direction.Y;
    }

    public void SetForcedMovement(bool forcedMovement)
    {
        this.player.Player.ForcedMovement = forcedMovement;
    }

    public void SetPlayer(Entity player)
    {
        this.player = player;
    }

    public void SetSpeed(float speed)
    {
        this.player.Speed = speed;
    }

    public void StopMovement()
    {
        this.player.Direction.X = 0f;
        this.player.Direction.Y = 0f;
    }

    public void SetGameState(Entity serverPlayer, long timestamp)
    {
        this.gameState.player = serverPlayer;
        this.gameState.timestamp = timestamp;
    }

    private Vector3 ClampIfOutOfMap(Vector3 newPosition, float playerRadius)
    {
        Vector3 mapCenterPosition = new Vector3(0, 0, 0);
        float playerDistanceFromMapCenter =
            Vector3.Distance(newPosition, mapCenterPosition) + playerRadius;

        if (playerDistanceFromMapCenter > mapRadius)
        {
            Vector3 fromOriginToObject = newPosition - mapCenterPosition;
            fromOriginToObject *= mapRadius / playerDistanceFromMapCenter;
            newPosition = mapCenterPosition + fromOriginToObject;
        }

        return newPosition;
    }

    private void processCollisions()
    {
        foreach (Entity obstacle in GameServerConnectionManager.Instance.obstacles)
        {
            switch (obstacle.Shape)
            {
                case "circle":
                    float distance = PositionUtils.DistanceToPosition(
                        player.Position,
                        obstacle.Position
                    );
                    if (distance <= player.Radius + obstacle.Radius)
                    {
                        Position normalized_direction = PositionUtils.NormalizedPosition(
                            PositionUtils.SubPosition(player.Position, obstacle.Position)
                        );
                        player.Position.X =
                            obstacle.Position.X
                            + (normalized_direction.X * player.Radius)
                            + (normalized_direction.X * obstacle.Radius);
                        player.Position.Y =
                            obstacle.Position.Y
                            + (normalized_direction.Y * player.Radius)
                            + (normalized_direction.Y * obstacle.Radius);
                    }

                    break;
                case "polygon":
                    (bool collided, Position direction, float depth) = SAT.IntersectCirclePolygon(
                        player,
                        obstacle
                    );

                    if (collided)
                    {
                        player.Position.X += direction.X * depth;
                        player.Position.Y += direction.Y * depth;
                    }

                    break;
                default:
                    Debug.Log("Missing obstacle shape");
                    break;
            }
        }
    }
}
