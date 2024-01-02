using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameHandler : MonoBehaviour
{
    private const string PLAYER_NAME_KEY = "playerName";
    private const string PLAYER_NAME_PLACEHOLDER = "Player Name";

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

    [SerializeField]
    TextMeshProUGUI playerNameText;

    public string playerName;

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
        PlayerPrefs.SetString(PLAYER_NAME_KEY, this.playerName);
    }

    public void UpdateUsername()
    {
        this.SetPlayerName();
        this.UpdateUsername(GetPlayerName());
    }

    public string GetPlayerName()
    {
        return this.playerName;
    }

    public void Show()
    {
        if (PlayerPrefs.HasKey(PLAYER_NAME_KEY))
        {
            this.playerName = PlayerPrefs.GetString(PLAYER_NAME_KEY);
            this.placeholder.text = this.playerName;
        }
        else
        {
            this.placeholder.text = PLAYER_NAME_PLACEHOLDER;
        }
        this.playerNameHandler.SetActive(true);
        this.playerNameHandler.GetComponent<CanvasGroup>().alpha = 1;
    }

    public void ClearPlaceholder()
    {
        if (this.placeholder.text == PLAYER_NAME_PLACEHOLDER)
        {
            this.placeholder.text = "";
        }
    }

    public void Hide()
    {
        this.playerNameHandler.SetActive(false);
    }

    public void UpdateUsername(string newUsername)
    {
        StartCoroutine(
            ServerUtils.SetUsername(
                newUsername,
                response =>
                {
                    LobbyConnection.Instance.username = response.username;
                    this.Hide();
                    this.playerNameText.text = response.username;
                },
                error =>
                {
                    Errors.Instance.HandleNetworkError("Error", error);
                }
            )
        );
    }
}
