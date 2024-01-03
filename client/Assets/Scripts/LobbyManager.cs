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

    public void Back()
    {
        ServerConnection.Instance.Init();
        this.LevelName = MAIN_SCENE_NAME;
        SceneManager.LoadScene(this.LevelName);
    }

    private void Update()
    {
        if (
            !String.IsNullOrEmpty(ServerConnection.Instance.GameSession)
            && SceneManager.GetActiveScene().name == LOBBY_SCENE_NAME
        )
        {
            ServerConnection.Instance.StartGame();
            SceneManager.LoadScene(BATTLE_SCENE_NAME);
        }
    }
}
