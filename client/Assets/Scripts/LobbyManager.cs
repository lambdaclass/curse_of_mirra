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
    bool loadScene = false;

    public static string LevelSelected;

    void Start()
    {
        StartCoroutine(WaitForLobbyJoin());
    }

    private void Update()
    {
        if (
            !string.IsNullOrEmpty(SessionParameters.GameId)
            && SceneManager.GetActiveScene().name == LOBBY_SCENE_NAME
            && loadScene
        )
        {
            Debug.Log("Loading battle scene");
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
        // ServerConnection.Instance.Init();
        this.LevelName = MAIN_SCENE_NAME;
        SceneManager.LoadScene(this.LevelName);
    }

    public IEnumerator WaitForLobbyJoin()
    {
        ServerConnection.Instance.JoinLobby();
        yield return new WaitUntil(
            () => !string.IsNullOrEmpty(ServerConnection.Instance.LobbySession)
        );
        loadScene = true;
        Debug.Log("Load battle scene");
    }
}
