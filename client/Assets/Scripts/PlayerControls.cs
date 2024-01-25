using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public void SendJoystickValues(float x, float y)
    {
        // if (x != 0 || y != 0)
        // {
        var valuesToSend = new Direction { X = x, Y = y };
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        GameServerConnectionManager.Instance.SendMove(x, y, timestamp);

        ClientPrediction.PlayerInput playerInput = new ClientPrediction.PlayerInput
        {
            joystick_x_value = x,
            joystick_y_value = y,
            timestamp = timestamp,
        };
        GameServerConnectionManager.Instance.clientPrediction.putPlayerInput(playerInput);
        // }
    }

    bool ShouldSendMovement(float x, float y, float lastXSent, float lastYSent)
    {
        return (x != lastXSent || y != lastYSent);
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

        SendJoystickValues(x, y);

        return (x, y);
    }

    public bool KeysPressed()
    {
        return Input.GetKey(KeyCode.W)
            || Input.GetKey(KeyCode.A)
            || Input.GetKey(KeyCode.D)
            || Input.GetKey(KeyCode.S);
    }

    public bool JoytickUsed(float x, float y)
    {
        return x != 0 || y != 0;
    }
}
