using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    // I think this should be an instance method
    // instead of void.
    public static void AttackIfInRange(int PlayerId)
    {
        var clientAction = new ClientAction { Action = Action.AutoAttack, Target = PlayerId };
        SocketConnectionManager.Instance.SendAction(clientAction);
    }

    public static void BasicAttack(Vector3 direction)
    {
        RelativePosition relativePosition = new RelativePosition
        {
            X = (long)(direction.x * 100),
            Y = (long)(direction.z * 100)
        };

        var clientAction = new ClientAction { Action = Action.BasicAttack, Position = relativePosition };
        SocketConnectionManager.Instance.SendAction(clientAction);
    }

    public void SendJoystickValues(float x, float y)
    {
        if (x != 0 || y != 0)
        {
            var valuesToSend = new JoystickValues { X = x, Y = y };
            var clientAction = new ClientAction { Action = Action.MoveWithJoystick, MoveDelta = valuesToSend };
            SocketConnectionManager.Instance.SendAction(clientAction);
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
