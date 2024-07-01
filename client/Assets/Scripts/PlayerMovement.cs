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
        public long timestamp;
        public long deltaTime;
    }

    public List<Movement> movements = new List<Movement>();

    public Entity player = new Entity();

    public long lastTimestamp = 0;
    public long currentTimestamp = 0;

    public float mapRadius;

    public struct ServerInfo
    {
        public Entity player;
        public long timestamp;
    }

    public Dictionary<long, bool> timestampsRead = new Dictionary<long, bool>();

    public ServerInfo gameState;

    public void MovePlayer()
    {
        long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        long deltaTime = now - lastTimestamp;

        player.Position.X += player.Direction.X * player.Speed * deltaTime;
        player.Position.Y += player.Direction.Y * player.Speed * deltaTime;

        ClampIfOutOfMap();
        processCollisions();

        lastTimestamp = now;

        // Refactor this: ESTE VECTOR CRECE AL INFINITO!! BORRAR LAS VIEJAS {
        Movement movement = new Movement
        {
            position = new Position { X = player.Position.X, Y = player.Position.Y },
            direction = new Direction { X = player.Direction.X, Y = player.Direction.Y },
            speed = player.Speed,
            timestamp = currentTimestamp,
            deltaTime = deltaTime,
        };
        movements.Add(movement);
        // }
    }

    public void ReconciliatePlayer(float reconciliationDistance)
    {
        int index = 0;
        while (movements[index].timestamp < gameState.timestamp && index < movements.Count - 1)
        {
            index += 1;
        }

        // Refactor this: ESTE DICCIONARIO CRECE AL INFINITO!! VER DE USAR UN CAMPO EN EL STRUCT MOVEMENT {
        if (timestampsRead.ContainsKey(movements[index].timestamp))
        {
            return;
        }

        timestampsRead[movements[index].timestamp] = true;
        // }

        float distance = Vector3.Distance(
            new Vector3(movements[index].position.X, 0, movements[index].position.Y),
            new Vector3(gameState.player.Position.X, 0, gameState.player.Position.Y)
        );

        if (distance > reconciliationDistance)
        {
            player.Position = gameState.player.Position;
            for (int i = index + 1; i < movements.Count; i++)
            {
                Movement movement = movements[i];
                long now = lastTimestamp;
                if (i < movements.Count - 1)
                {
                    now = movements[i + 1].timestamp;
                }

                player.Position.X += movement.direction.X * movement.speed * movement.deltaTime;
                player.Position.Y += movement.direction.Y * movement.speed * movement.deltaTime;
            }
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

    private void ClampIfOutOfMap()
    {
        Vector3 mapCenterPosition = new Vector3(0, 0, 0);
        Vector3 playerPositionVector = new Vector3(player.Position.X, 0, player.Position.Y);
        float playerDistanceFromMapCenter =
            Vector3.Distance(playerPositionVector, mapCenterPosition) + player.Radius;

        if (playerDistanceFromMapCenter > mapRadius)
        {
            Vector3 fromOriginToObject = playerPositionVector - mapCenterPosition;
            fromOriginToObject *= mapRadius / playerDistanceFromMapCenter;
            Vector3 newPosition = mapCenterPosition + fromOriginToObject;

            player.Position.X = newPosition.x;
            player.Position.Y = newPosition.z;
        }
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
