using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public void SendJoystickValues(float x, float y)
    {
        if (x != 0 || y != 0)
        {
            GameServerConnectionManager.Instance.SendMove(x, y);

            // ClientPrediction.PlayerInput playerInput = new ClientPrediction.PlayerInput
            // {
            //     joystick_x_value = x,
            //     joystick_y_value = y,
            //     timestamp = timestamp,
            // };
            // GameServerConnectionManager.Instance.clientPrediction.putPlayerInput(playerInput);
        }
    }

    public (float, float) SendAction()
    {
        float x = 0;
        float y = 0;
        if (Input.GetKey(KeyCode.W))
        {
            y += 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            x += -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            x += 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            y += -1f;
        }
        if (x != 0 || y != 0)
        {
            SendJoystickValues(x, y);
        }
        return (x, y);
    }
}
