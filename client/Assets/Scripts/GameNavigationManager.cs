using System.Collections;
using MoreMountains.TopDownEngine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameNavigationManager : LevelSelector
{
    public static GameNavigationManager Instance;
    private const string BATTLE_SCENE_NAME = "Battle";
    private const string LOBBY_SCENE_NAME = "Lobby";
    private const string CHARACTER_INFO_SCENE_NAME = "CharacterInfo";
    private const string MAIN_SCENE_NAME = "MainScreen";
    private const string TITLE_SCENE_NAME = "TitleScreen";

    void Start()
    {
        if(SceneManager.GetActiveScene().name == LOBBY_SCENE_NAME){
            StartCoroutine(Utils.WaitForBattleCreation(LOBBY_SCENE_NAME, BATTLE_SCENE_NAME, "join"));
        }
    }
    public void ExitGame(string goToScene){
        Utils.BackToLobbyFromGame(goToScene);
    }
}
