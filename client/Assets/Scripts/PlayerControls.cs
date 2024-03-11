using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public void SendJoystickValues(float x, float y)
    {
        if (
            ShouldSendMovement(
                x,
                y,
                GameServerConnectionManager.Instance.clientPrediction.lastXSent,
                GameServerConnectionManager.Instance.clientPrediction.lastYSent
            )
        )
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
            };
            GameServerConnectionManager.Instance.clientPrediction.EnqueuePlayerInput(playerInput);
        }
    }

    bool ShouldSendMovement(float x, float y, float lastXSent, float lastYSent)
    {
        float movementThreshold = 1f;
        //Fetch the first GameObject's position
        Vector2 currentDirection = new Vector2(x, y);
        //Fetch the second GameObject's position
        Vector2 lastDirection = new Vector2(lastXSent, lastYSent);
        //Find the angle for the two Vectors
        float angleBetweenDirections = Vector2.Angle(currentDirection, lastDirection);

        bool movedFromStatic = (lastXSent == 0 && lastYSent == 0 && (x != 0 || y != 0));
        bool stoppedMoving = (x == 0 && y == 0 && (lastXSent != 0 || lastYSent != 0));
        bool changedDirection = (angleBetweenDirections > movementThreshold);
        // Here we can add a validaion to check if
        // the movement is significant enough to be sent to the server
        return (movedFromStatic || stoppedMoving || changedDirection);
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
