using TMPro;
using UnityEngine;

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
        this.playerText.text = $"{name}";
        this.youText = ServerConnection.Instance.playerId == id ? "YOU" : null;
        string separator = this.youText != null ? " / " : null;

        this.playerRollText.text = this.youText;
    }

    public void updateText()
    {
        string separator = this.youText != null ? " / " : null;

        this.playerRollText.text = this.youText;
    }
}
