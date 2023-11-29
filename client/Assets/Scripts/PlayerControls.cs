using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    float lastXSent = 0;
    float lastYSent = 0;

    long lastTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

    public void SendJoystickValues(float x, float y)
    {
        float angle;
        bool moving = x != 0 || y != 0;
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        if (x != lastXSent || y != lastYSent)
        {
            angle = moving
                ? Mathf.Atan2(y, x) * Mathf.Rad2Deg
                : Mathf.Atan2(lastYSent, lastXSent) * Mathf.Rad2Deg;
            Move moveAction = new Move { Angle = angle, Moving = moving };
            GameAction gameAction = new GameAction { Move = moveAction, Timestamp = timestamp };

            SocketConnectionManager.Instance.SendGameAction(gameAction);

            ClientPrediction.PlayerInput playerInput = new ClientPrediction.PlayerInput
            {
                joystick_x_value = x,
                joystick_y_value = y,
                timestamp = timestamp,
            };
            SocketConnectionManager.Instance.clientPrediction.putPlayerInput(playerInput);
            lastXSent = x;
            lastYSent = y;
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

    public static float getBackendCharacterSpeed(ulong playerId)
    {
        string charName = Utils.GetGamePlayer(playerId).CharacterName;
        var chars = LobbyConnection.Instance.engineServerSettings.Characters;

        foreach (var character in chars)
        {
            if (charName.ToLower() == character.Name.ToLower())
            {
                Debug.Log($"config speed is: {character.BaseSpeed}");
                return character.BaseSpeed;
            }
        }
        return 0f;
    }
}
