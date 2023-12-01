using System;
using System.Collections;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : LevelSelector
{
    private const string BATTLE_SCENE_NAME = "Battle";
    private const string LOBBY_SCENE_NAME = "Lobby";
    private const string MAIN_SCENE_NAME = "MainScreen";
    private const string LOBBIES_BACKGROUND_MUSIC = "LobbiesBackgroundMusic";

    public static string LevelSelected;

    public override void GoToLevel()
    {
        base.GoToLevel();
        gameObject.GetComponent<MMTouchButton>().DisableButton();
    }

    public void GameStart()
    {
        // StartCoroutine(CreateGame());
        this.LevelName = BATTLE_SCENE_NAME;
        StartCoroutine(Utils.WaitForGameCreation(this.LevelName));
    }

    public void Back()
    {
        LobbyConnection.Instance.Init();
        this.LevelName = MAIN_SCENE_NAME;
        SceneManager.LoadScene(this.LevelName);
    }

    public void BackToLobbyAndCloseConnection()
    {
        // Websocket connection is closed as part of Init() destroy;
        SocketConnectionManager.Instance.Init();
        DestroySingletonInstances();
        Back();
    }

    public void BackToLobbyFromGame()
    {
        Destroy(GameObject.Find(LOBBIES_BACKGROUND_MUSIC));
        BackToLobbyAndCloseConnection();
    }

    public void SelectMap(string mapName)
    {
        this.LevelName = mapName;
        LevelSelected = mapName;
    }

    private void DestroySingletonInstances()
    {
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }
    }

    private void Update()
    {
        if (
            !String.IsNullOrEmpty(LobbyConnection.Instance.GameSession)
            && SceneManager.GetActiveScene().name == LOBBY_SCENE_NAME
        )
        {
            LobbyConnection.Instance.StartGame();
            SceneManager.LoadScene(BATTLE_SCENE_NAME);
        }
    }
}
