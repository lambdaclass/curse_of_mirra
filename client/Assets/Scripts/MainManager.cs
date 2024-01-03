using System;
using System.Collections;
using MoreMountains.TopDownEngine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : LevelSelector
{
    public static MainManager Instance;

    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Instance = this;
    }

    public void JoinLobby()
    {
        StartCoroutine(WaitForLobbyCreation());
    }

    public void ConnectToLobby(string idHash)
    {
        StartCoroutine(WaitForLobbyJoin(idHash));
    }

    public IEnumerator WaitForLobbyCreation()
    {
        ServerConnection.Instance.JoinLobby();
        yield return new WaitUntil(
            () =>
                !string.IsNullOrEmpty(ServerConnection.Instance.LobbySession)
                && ServerConnection.Instance.playerId != UInt64.MaxValue
        );
        SceneManager.LoadScene("Lobby");
    }

    public IEnumerator WaitForLobbyJoin(string idHash)
    {
        ServerConnection.Instance.ConnectToLobby(idHash);
        yield return new WaitUntil(() => ServerConnection.Instance.playerId != UInt64.MaxValue);
        SceneManager.LoadScene("Lobby");
    }
}
