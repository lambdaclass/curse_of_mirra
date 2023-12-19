using System;
using UnityEngine;
using Communication.Protobuf;

public class PlayerControls : MonoBehaviour
{
    public void SendJoystickValues(float x, float y)
    {
        bool moving = x != 0 || y != 0;

        (float lastXSent, float lastYSent) =
            SocketConnectionManager.Instance.clientPrediction.GetLastSentDirection();

        float lastAngleSent = Mathf.Atan2(lastYSent, lastXSent) * Mathf.Rad2Deg;
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        float angle = moving ? Mathf.Atan2(y, x) * Mathf.Rad2Deg : lastAngleSent;
        float difference = Math.Abs(angle - lastAngleSent);

        if (ShouldSendMovement(x, y, difference, lastXSent, lastYSent))
        {
            Move moveAction = new Move { Angle = angle, Moving = moving };
            GameAction gameAction = new GameAction { Move = moveAction, Timestamp = timestamp };

            SocketConnectionManager.Instance.SendGameAction(gameAction);

            ClientPrediction.PlayerInput playerInput = new ClientPrediction.PlayerInput
            {
                joystick_x_value = x,
                joystick_y_value = y,
                startMovementTimestamp = timestamp,
                timestampId = timestamp
            };
            SocketConnectionManager.Instance.clientPrediction.PutPlayerInput(playerInput);
        }
    }

    bool ShouldSendMovement(float x, float y, float difference, float lastXSent, float lastYSent)
    {
        return (x != lastXSent || y != lastYSent) && ((difference <= 0.01f) || difference > 5);
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

    public static float getBackendCharacterSpeed(ulong playerId)
    {
        string charName = Utils.GetGamePlayer(playerId).CharacterName;
        var chars = LobbyConnection.Instance.engineServerSettings.Characters;

        foreach (var character in chars)
        {
            if (charName.ToLower() == character.Name.ToLower())
            {
                return character.BaseSpeed;
            }
        }
        return 0f;
    }
}
