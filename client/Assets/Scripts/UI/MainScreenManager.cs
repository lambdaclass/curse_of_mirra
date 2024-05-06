using System.Collections;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScreenManager : MonoBehaviour
{
    private const string BATTLE_SCENE_NAME = "Battle";
    private const string MAIN_SCENE_NAME = "MainScreen";

    [SerializeField]
    UIModelManager modelManager;

    [SerializeField]
    TextMeshProUGUI playerName;

    [SerializeField]
    TextMeshProUGUI currentPlayerName;
    
    string characterNameToGo;

    void Start()
    {
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
        StartCoroutine(ServerConnection.Instance.WaitForBattleCreation(MAIN_SCENE_NAME, BATTLE_SCENE_NAME, "quick_game"));
    }
}
