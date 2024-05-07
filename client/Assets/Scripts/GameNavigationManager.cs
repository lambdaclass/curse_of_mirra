using System.Collections;
using MoreMountains.TopDownEngine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameNavigationManager : LevelSelector
{
    private const string BATTLE_SCENE_NAME = "Battle";
    private const string LOBBY_SCENE_NAME = "Lobby";
    private const string MAIN_SCENE_NAME = "MainScreen";

    void Start()
    {
        if(SceneManager.GetActiveScene().name == LOBBY_SCENE_NAME){
            StartCoroutine(Utils.WaitForBattleCreation(LOBBY_SCENE_NAME, BATTLE_SCENE_NAME, "join"));
        }
    }
    public void ExitGame(string goToScene){
        Utils.BackToLobbyFromGame(goToScene);
    }

    public void JoinLobby()
    {
        SceneManager.LoadScene(LOBBY_SCENE_NAME);
    }

    public void QuickGame()
    {
        StartCoroutine(Utils.WaitForBattleCreation(MAIN_SCENE_NAME, BATTLE_SCENE_NAME, "quick_game"));
    }
}
