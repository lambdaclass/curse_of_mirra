using System.Collections;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScreenManager : MonoBehaviour
{
    [SerializeField]
    UIModelManager modelManager;

    [SerializeField]
    TextMeshProUGUI playerName;

    [SerializeField]
    TextMeshProUGUI currentPlayerName;
    
    string characterNameToGo;

    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        CharactersManager
            .Instance
            .SetGoToCharacter(ServerConnection.Instance.selectedCharacterName);
        characterNameToGo = ServerConnection.Instance.selectedCharacterName;
        modelManager.SetModel(characterNameToGo);
        playerName.text = PlayerPrefs.GetString("playerName");
        currentPlayerName.text = "Current name: " + PlayerPrefs.GetString("playerName");
    }

    public void GoToCharacteInfo()
    {
        Utils.GoToCharacterInfo(characterNameToGo, "CharacterInfo");
    }

    public void JoinLobby()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void QuickGame()
    {
        StartCoroutine(WaitForBattleCreation());
    }

    public IEnumerator WaitForBattleCreation()
    {
        ServerConnection.Instance.JoinGame("quick_game");
        yield return new WaitUntil(
            () =>
                !string.IsNullOrEmpty(ServerConnection.Instance.LobbySession)
                && !string.IsNullOrEmpty(SessionParameters.GameId)
        );
        SceneManager.LoadScene("Battle");
    }
}
