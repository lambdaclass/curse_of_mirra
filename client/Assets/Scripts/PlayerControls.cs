using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    float lastXSent;
    float lastYSent;

    public void SendJoystickValues(float x, float y)
    {
        if (ShouldSendMovement(x, y, lastXSent, lastYSent))
        {
            var valuesToSend = new Direction { X = x, Y = y };
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            GameServerConnectionManager.Instance.SendMove(x, y, timestamp);

            ClientPrediction.PlayerInput playerInput = new ClientPrediction.PlayerInput
            {
                joystick_x_value = x,
                joystick_y_value = y,
                timestampId = timestamp,
                startTimestamp = timestamp,
                endTimestamp = 0,
                position = new Position { X = 0, Y = 0 }
            };
            GameServerConnectionManager.Instance.clientPrediction.EnqueuePlayerInput(playerInput);
            lastXSent = x;
            lastYSent = y;
        }
    }

    bool ShouldSendMovement(float x, float y, float lastXSent, float lastYSent)
    {
        // Here we can add a validaion to check if
        // the movement is significant enough to be sent to the server
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
