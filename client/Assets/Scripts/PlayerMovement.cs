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

    public void MovePlayer()
    {
        if (movements.Count > 0)
        {
            long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            long deltaTime = now - lastTimestamp;
            player.Position.X +=
                movements[movements.Count - 1].direction_x * player.Speed * deltaTime;
            player.Position.Y +=
                movements[movements.Count - 1].direction_y * player.Speed * deltaTime;

            player.Direction.X = movements[movements.Count - 1].direction_x;
            player.Direction.Y = movements[movements.Count - 1].direction_y;

            lastTimestamp = now;
        }
    }

    public void AddMovement(Movement movement)
    {
        movements.Add(movement);
    }

    public void SetPlayer(Entity player)
    {
        this.player = player;
    }
}
