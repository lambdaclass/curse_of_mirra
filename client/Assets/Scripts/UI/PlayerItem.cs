using System.Collections;
using System.Collections.Generic;
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
        if (id == 1 && LobbyConnection.Instance.playerId == id)
        {
            this.playerRollText.text = $"HOST / YOU";
        }
        else if (id == 1)
        {
            this.playerRollText.text = $"HOST ";
        }
        else if (LobbyConnection.Instance.playerId == id)
        {
            this.playerRollText.text = "YOU";
        }
    }
}
