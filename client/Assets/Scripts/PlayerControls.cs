using System;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    float lastXSent = 0;
    float lastYSent = 0;

    long lastTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

    public void SendJoystickValues(float x, float y)
    {
        bool moving = (x != 0 || y != 0);
        float lastAngleSent = Mathf.Atan2(lastYSent, lastXSent) * Mathf.Rad2Deg;
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        float angle = moving
            ? Mathf.Atan2(y, x) * Mathf.Rad2Deg
            : lastAngleSent;
        float difference = Math.Abs(angle - lastAngleSent);

        if (ShouldSendMovement(x, y, difference))
        {
            Move moveAction = new Move { Angle = angle, Moving = moving };
            GameAction gameAction = new GameAction { Move = moveAction, Timestamp = timestamp };

            SocketConnectionManager.Instance.SendGameAction(gameAction);

            ClientPrediction.PlayerInput playerInput = new ClientPrediction.PlayerInput
            {
                joystick_x_value = x,
                joystick_y_value = y,
                timestamp = timestamp,
            };
            // Debug.Log("Angle: " + angle + " Timestamp: " + timestamp);
            SocketConnectionManager.Instance.clientPrediction.putPlayerInput(playerInput);
            lastXSent = x;
            lastYSent = y;
            lastTimestamp = timestamp;
        }
    }

    bool ShouldSendMovement(float x, float y, float difference) {
        return (x != lastXSent || y != lastYSent) && (difference == 0 || difference > 5);
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
                return character.BaseSpeed;
            }
        }
        return 0f;
    }
}
