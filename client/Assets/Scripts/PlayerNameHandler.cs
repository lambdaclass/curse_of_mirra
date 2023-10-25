using UnityEngine;
using UnityEngine.UI;

public class PlayerNameHandler : MonoBehaviour
{
    [SerializeField]
    GameObject playerNameHandler;

    [SerializeField]
    Text playerNameInput;

    [SerializeField]
    Sprite selectedButtonSprite;

    private string playerName;

    public void SetPlayerName()
    {
        this.playerName = playerNameInput.text.Trim();
        GetComponent<Image>().sprite = selectedButtonSprite;
        PlayerPrefs.SetString("playerName", playerName);
        this.Hide();
    }

    public string GetPlayerName()
    {
        return this.playerName;
    }

    public void Show()
    {
        this.playerNameHandler.SetActive(true);
    }

    private void Hide()
    {
        this.playerNameHandler.SetActive(false);
    }
}
