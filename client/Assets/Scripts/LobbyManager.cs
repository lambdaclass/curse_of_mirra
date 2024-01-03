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

    private void Update()
    {
        if (
            !String.IsNullOrEmpty(ServerConnection.Instance.GameSession)
            && SceneManager.GetActiveScene().name == LOBBY_SCENE_NAME
        )
        {
            SceneManager.LoadScene(BATTLE_SCENE_NAME);
        }
    }

    public void BackToLobbyFromGame()
    {
        Destroy(GameObject.Find(LOBBIES_BACKGROUND_MUSIC));
        BackToLobbyAndCloseConnection();
    }

    public void BackToLobbyAndCloseConnection()
    {
        // Websocket connection is closed as part of Init() destroy;
        GameServerConnectionManager.Instance.Init();
        DestroySingletonInstances();
        Back();
    }

    private void DestroySingletonInstances()
    {
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }
    }

    public void Back()
    {
        ServerConnection.Instance.Init();
        this.LevelName = MAIN_SCENE_NAME;
        SceneManager.LoadScene(this.LevelName);
    }
}
