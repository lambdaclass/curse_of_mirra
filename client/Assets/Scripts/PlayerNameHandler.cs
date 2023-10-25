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

    [SerializeField]
    GameObject errorMessage;

    [SerializeField]
    InputField placeholder;

    private string playerName;

    public void SetPlayerName()
    {
        this.playerName = playerNameInput.text.Trim();
        if (this.playerName == "")
        {
            this.errorMessage.SetActive(true);
            return;
        }
        this.errorMessage.SetActive(false);
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
        if (PlayerPrefs.HasKey("playerName"))
        {
            this.playerName = PlayerPrefs.GetString("playerName");
            this.placeholder.text = this.playerName;
        }
        else
        {
            this.placeholder.text = "Player Name";
        }
        this.playerNameHandler.SetActive(true);
    }

    private void Hide()
    {
        this.playerNameHandler.SetActive(false);
    }
}
