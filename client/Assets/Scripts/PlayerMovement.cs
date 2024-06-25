using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement
{
    public struct Movement
    {
        public float direction_x;
        public float direction_y;
        public float speed;
    }

    public List<Movement> movements = new List<Movement>();

    public Entity player = new Entity();

    public long lastTimestamp = 0;

    public float mapRadius;

    public void MovePlayer()
    {
        if (player.Player.ForcedMovement)
        {
            return;
        }
        if (movements.Count > 0)
        {
            long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            long deltaTime = now - lastTimestamp;
            player.Position.X +=
                movements[movements.Count - 1].direction_x * player.Speed * deltaTime;
            player.Position.Y +=
                movements[movements.Count - 1].direction_y * player.Speed * deltaTime;

            // Refactor this {
            Vector3 newPosition = new Vector3(player.Position.X, 0, player.Position.Y);
            newPosition = ClampIfOutOfMap(newPosition, player.Radius);

            player.Position.X = newPosition.x;
            player.Position.Y = newPosition.z;
            processCollisions();
            // }

            player.Direction.X = movements[movements.Count - 1].direction_x;
            player.Direction.Y = movements[movements.Count - 1].direction_y;

            lastTimestamp = now;
        }
    }

    public void AddMovement(Movement movement)
    {
        if (player.Player.ForcedMovement)
        {
            return;
        }
        movements.Add(movement);
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
        PlayerMovement.Movement movement = new PlayerMovement.Movement
        {
            direction_x = 0f,
            direction_y = 0f,
            speed = player.Speed
        };
        AddMovement(movement);
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

    private void processCollisions(){
        foreach (Entity obstacle in GameServerConnectionManager.Instance.obstacles)
            {
                switch (obstacle.Shape)
                {
                    case "circle":
                        float distance = PositionUtils.DistanceToPosition(player.Position, obstacle.Position);
                        if (distance <= player.Radius + obstacle.Radius)
                        {
                            Position normalized_direction = PositionUtils.NormalizedPosition(
                                PositionUtils.SubPosition(player.Position, obstacle.Position)
                            );
                            player.Position.X = obstacle.Position.X + (normalized_direction.X * player.Radius) + (normalized_direction.X * obstacle.Radius);
                            player.Position.Y = obstacle.Position.Y + (normalized_direction.Y * player.Radius) + (normalized_direction.Y * obstacle.Radius);
                        }

                        break;
                    case "polygon":
                        (bool collided, Position direction, float depth) = SAT.IntersectCirclePolygon(player, obstacle);

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
