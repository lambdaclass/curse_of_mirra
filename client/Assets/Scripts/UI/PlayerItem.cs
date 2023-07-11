using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviour
{
    [SerializeField]
    public Text playerText;
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
        if (id == 1)
        {
            this.playerText.text = $"Player {id.ToString()} {characterName} HOST";
        }
        else
        {
            if (LobbyConnection.Instance.playerId == id)
            {
                this.playerText.text = $"Player {id.ToString()} {characterName} YOU";
            }
            else
            {
                this.playerText.text = $"Player {id.ToString()} {characterName}";
            }
        }
    }
}
