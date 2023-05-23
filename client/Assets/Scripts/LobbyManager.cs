using System;
using System.Collections;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : LevelSelector
{
    [SerializeField]
    GameObject playButton;

    public override void GoToLevel()
    {
        base.GoToLevel();
        gameObject.GetComponent<MMTouchButton>().DisableButton();
    }

    void Start()
    {
        if (LobbyConnection.Instance.playerId == 1)
        {
            playButton.SetActive(true);
        }
        else
        {
            playButton.SetActive(false);
        }
    }

    public void GameStart()
    {
        LobbyConnection.Instance.StartGame();
        SceneManager.LoadScene("BackendPlayground");
    }

    public void Back()
    {
        LobbyConnection.Instance.Init();
        SceneManager.LoadScene("Lobbies");
    }

    private void Update()
    {
        if (
            !String.IsNullOrEmpty(LobbyConnection.Instance.GameSession)
            && !LobbyConnection.Instance.gameStarted
        )
        {
            LobbyConnection.Instance.StartGame();
            SceneManager.LoadScene("BackendPlayground");
        }
    }
}
