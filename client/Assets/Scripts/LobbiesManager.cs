using System;
using System.Collections;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbiesManager : LevelSelector
{
    [SerializeField]
    public PlayerNameHandler playerNameHandler;
    public static LobbiesManager Instance;

    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Instance = this;
        if (PlayerPrefs.GetString("playerName") == "")
        {
            Debug.Log("Showing pop up");
            this.ShowPlayerNamePopUp();
        }
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
        LobbyConnection.Instance.Init();
        SceneManager.LoadScene("Lobbies");
    }

    public void Refresh()
    {
        LobbyConnection.Instance.Refresh();
        this.GetComponent<UIManager>().RefreshLobbiesList();
    }

    public void QuickGame()
    {
        LobbyConnection.Instance.QuickGame();
        StartCoroutine(Utils.WaitForGameCreation(this.LevelName));
    }

    public IEnumerator WaitForLobbyCreation()
    {
        LobbyConnection.Instance.CreateLobby();
        yield return new WaitUntil(
            () =>
                !string.IsNullOrEmpty(LobbyConnection.Instance.LobbySession)
                && LobbyConnection.Instance.playerId != UInt64.MaxValue
        );
        SceneManager.LoadScene("Lobby");
    }

    public void Reconnect()
    {
        LobbyConnection.Instance.Reconnect();
        SceneManager.LoadScene("CharacterSelection");
    }

    public IEnumerator WaitForLobbyJoin(string idHash)
    {
        LobbyConnection.Instance.ConnectToLobby(idHash);
        yield return new WaitUntil(() => LobbyConnection.Instance.playerId != UInt64.MaxValue);
        SceneManager.LoadScene("Lobby");
    }

    public void ShowPlayerNamePopUp()
    {
        this.playerNameHandler.Show();
    }
}
