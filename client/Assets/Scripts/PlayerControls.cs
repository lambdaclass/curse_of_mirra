using UnityEngine;

public class PlayerControls : MonoBehaviour
{
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
        if (Input.GetKey(KeyCode.E))
        {
            ClientAction action = new ClientAction { Action = Action.AttackAoe };
            SocketConnectionManager.Instance.SendAction(action);
        }
    }

    private static void SendAction(Action action, Direction direction)
    {
        ClientAction clientAction = new ClientAction { Action = action, Direction = direction };
        SocketConnectionManager.Instance.SendAction(clientAction);
    }
}
