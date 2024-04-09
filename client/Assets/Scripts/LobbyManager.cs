using System.Collections;
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

    void Start()
    {
        StartCoroutine(WaitForLobbyJoin());
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
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == LOBBY_SCENE_NAME);
        ServerConnection.Instance.JoinLobby();
        yield return new WaitUntil(
            () =>
                !string.IsNullOrEmpty(ServerConnection.Instance.LobbySession)
                && !string.IsNullOrEmpty(SessionParameters.GameId)
        );
        SceneManager.LoadScene(BATTLE_SCENE_NAME);
    }

    public void LeaveLobby(){
        ServerConnection.Instance.LeaveLobby();
        if(System.String.IsNullOrEmpty(ServerConnection.Instance.GameSession)){
            SceneManager.LoadScene("MainScreen");
        }
    }
}
