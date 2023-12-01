using System;
using System.Collections;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DevLobbiesManager : LevelSelector
{
    public static DevLobbiesManager Instance;

    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Instance = this;
    }

    public override void GoToLevel()
    {
        base.GoToLevel();
        gameObject.GetComponent<MMTouchButton>().DisableButton();
    }

    public void CreateLobby()
    {
        StartCoroutine(WaitForLobbyCreation());
    }

    public void ConnectToLobby(string idHash)
    {
        StartCoroutine(WaitForLobbyJoin(idHash));
    }

    public void Back()
    {
        DevLobbyConnection.Instance.Init();
        SceneManager.LoadScene("Lobbies");
    }

    public void Refresh()
    {
        DevLobbyConnection.Instance.Refresh();
        this.GetComponent<UIManager>().RefreshLobbiesList();
    }

    public void QuickGame()
    {
        DevLobbyConnection.Instance.QuickGame();
        StartCoroutine(Utils.WaitForGameCreation(this.LevelName));
    }

    public IEnumerator WaitForLobbyCreation()
    {
        DevLobbyConnection.Instance.CreateLobby();
        yield return new WaitUntil(
            () =>
                !string.IsNullOrEmpty(DevLobbyConnection.Instance.LobbySession)
                && DevLobbyConnection.Instance.playerId != UInt64.MaxValue
        );
        SceneManager.LoadScene("Lobby");
    }

    public void Reconnect()
    {
        DevLobbyConnection.Instance.Reconnect();
        SceneManager.LoadScene("CharacterSelection");
    }

    public IEnumerator WaitForLobbyJoin(string idHash)
    {
        DevLobbyConnection.Instance.ConnectToLobby(idHash);
        yield return new WaitUntil(() => DevLobbyConnection.Instance.playerId != UInt64.MaxValue);
        SceneManager.LoadScene("Lobby");
    }
}
