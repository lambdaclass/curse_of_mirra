using UnityEngine;
using System.Collections.Generic;
using System;

public class PlayerControls : MonoBehaviour
{
    public void SendJoystickValues(float x, float y)
    {
        if (x != 0 || y != 0)
        {
            var valuesToSend = new JoystickValues { X = x, Y = y };
            var clientAction = new ClientAction { Action = Action.MoveWithJoystick, MoveDelta = valuesToSend };
            SocketConnectionManager.Instance.SendAction(clientAction);
            Player p = SocketConnectionManager.Instance.gamePlayers[0];
            // let norm = f64::sqrt(x.powf(2.) + y.powf(2.));
    // (x / norm, y / norm)
            var norm = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
            var x_norm = (long) Math.Round(x / norm) * 3;
            var y_norm = (long) Math.Round(y / norm) * 3;
            // let mut new_position_x = old_x as i64 + (movement_vector_x.round() as i64);
            // let mut new_position_y = old_y as i64 + (movement_vector_y.round() as i64);

            // new_position_x = min(new_position_x, (self.board.height - 1) as i64);
            // new_position_x = max(new_position_x, 0);
            // new_position_y = min(new_position_y, (self.board.width - 1) as i64);
            // new_position_y = max(new_position_y, 0);


            Position pos = new Position();
            pos.X = (ulong) x;
            pos.Y = (ulong) y;
            p.Id = 1;
            p.Health = 100;
            p.Position.X = (ulong) ((long) p.Position.X - y_norm);
            p.Position.Y = (ulong) ((long) p.Position.Y + x_norm);;
            p.AoePosition = pos;
            print("CLIENT pos X: " + p.Position.X + "  Y: " + p.Position.Y);
            SocketConnectionManager.Instance.gamePlayers = new List<Player> { p };
        }
    }
    public void SendAction()
    {
        if (Input.GetKey(KeyCode.W))
        {
            SendAction(Action.Move, Direction.Up);
        }
        if (Input.GetKey(KeyCode.A))
        {
            SendAction(Action.Move, Direction.Left);
        }
        if (Input.GetKey(KeyCode.D))
        {
            SendAction(Action.Move, Direction.Right);
        }
        if (Input.GetKey(KeyCode.S))
        {
            SendAction(Action.Move, Direction.Down);
        }
    }

    private static void SendAction(Action action, Direction direction)
    {
        ClientAction clientAction = new ClientAction { Action = action, Direction = direction };
        SocketConnectionManager.Instance.SendAction(clientAction);
    }
}
