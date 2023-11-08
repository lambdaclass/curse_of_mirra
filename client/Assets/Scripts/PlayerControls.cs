using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public void SendJoystickValues(float x, float y)
    {
        if (x != 0 || y != 0)
        {
            var valuesToSend = new RelativePosition { X = x, Y = y };
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            Move moveAction = new Move { Angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg };
            GameAction gameAction = new GameAction { Move = moveAction, Timestamp = timestamp };

            SocketConnectionManager.Instance.SendGameAction(gameAction);

            ClientPrediction.PlayerInput playerInput = new ClientPrediction.PlayerInput
            {
                joystick_x_value = x,
                joystick_y_value = y,
                timestamp = timestamp,
            };
            SocketConnectionManager.Instance.clientPrediction.putPlayerInput(playerInput);
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
        // if (SocketConnectionManager.Instance.selectedCharacters.ContainsKey(playerId))
        // {
        // var charName = SocketConnectionManager.Instance.selectedCharacters[playerId];
        var chars = LobbyConnection.Instance.engineServerSettings.Characters;

        foreach (var character in chars)
        {
            if (charName.ToLower() == character.Name.ToLower())
            {
                return character.BaseSpeed;
            }
        }
        // }
        return 0f;
    }
}
