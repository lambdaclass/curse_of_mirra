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

    private string playerName;
    private string hostText;
    private string youText;

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

    public void SetPlayerItemText(string name)
    {
        this.playerText.text = $"Player {name}";

        this.hostText = LobbyConnection.Instance.hostId == id ? "HOST" : null;
        this.youText = LobbyConnection.Instance.playerId == id ? "YOU" : null;
        string separator = this.hostText != null && this.youText != null ? " / " : null;

        this.playerRollText.text = this.hostText + separator + this.youText;
    }

    public void updateText() {
        this.hostText = LobbyConnection.Instance.hostId == id ? "HOST" : null;
        string separator = this.hostText != null && this.youText != null ? " / " : null;

        this.playerRollText.text = this.hostText + separator + this.youText;

    }
}
