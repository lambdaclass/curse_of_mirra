using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SetPlayerName : MonoBehaviour
{
    [SerializeField]
    Text playerNameInput;

    [SerializeField]
    Sprite selectedButtonSprite;

    public static string playerName;

    public void SetServerIp()
    {
        playerName = playerNameInput.text.Trim();
        GetComponent<Image>().sprite = selectedButtonSprite;
        LobbyConnection.Instance.Refresh();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static string GetServerIp()
    {
        return playerName;
    }

    public void HidePlayerNamePopUp()
    {
        gameObject.SetActive(false);
    }
}
