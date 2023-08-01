using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI playerText;

    [SerializeField]
    public TextMeshProUGUI playerRollText;
    public ulong id;
    public string characterName;

    public ulong GetId()
    {
        return id;
    }

    public void SetId(ulong id)
    {
        this.id = id;
    }

    public string GetName()
    {
        return name;
    }

    public void SetCharacterName(string name)
    {
        this.characterName = name;
    }

    public void SetPlayerItemText()
    {
        this.playerText.text = $"Player {id.ToString()} {characterName} ";
        if (LobbyConnection.Instance.hostId == id && LobbyConnection.Instance.playerId == id)
        {
            this.playerRollText.text = $"HOST / YOU";
        }
        else if (LobbyConnection.Instance.hostId == id)
        {
            this.playerRollText.text = $"HOST ";
        }
        else if (LobbyConnection.Instance.playerId == id)
        {
            this.playerRollText.text = "YOU";
        }
    }
}
