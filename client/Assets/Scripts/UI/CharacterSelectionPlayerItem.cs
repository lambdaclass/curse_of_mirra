using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterSelectionPlayerItem : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI playerText;

    [SerializeField]
    public TextMeshProUGUI characterText;

    [SerializeField]
    public Image characterImage;
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
        this.characterText.text = name;
        this.characterName = name;
    }

    public void SetPlayerItemText()
    {
        if (id == 1)
        {
            this.playerText.text = $"Player HOST ";
        }
        else
        {
            this.playerText.text = $"Player ";
        }

        if (LobbyConnection.Instance.playerId == id)
        {
            this.playerText.color = new Color32(196, 121, 217, 255);
        }
    }

    public void SetSprite(Sprite characterImage)
    {
        this.characterImage.sprite = characterImage;
    }
}
