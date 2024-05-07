using System.Collections;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScreenManager : MonoBehaviour
{
    private const string BATTLE_SCENE_NAME = "Battle";
    private const string LOBBY_SCENE_NAME = "Lobby";
    private const string MAIN_SCENE_NAME = "MainScreen";
    private const string CHARACTER_INFO_SCENE_NAME = "CharacterInfo";
    private const string TITLE_SCENE_NAME = "TitleScreen";

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
        Utils.GoToCharacterInfo(characterNameToGo, CHARACTER_INFO_SCENE_NAME);
    }

    public void JoinLobby()
    {
        SceneManager.LoadScene(LOBBY_SCENE_NAME);
    }

    public void QuickGame()
    {
        StartCoroutine(Utils.WaitForBattleCreation(MAIN_SCENE_NAME, BATTLE_SCENE_NAME, "quick_game"));
    }
    public void ChangeServer(){
        Utils.BackToLobbyFromGame(TITLE_SCENE_NAME);
    }
}
