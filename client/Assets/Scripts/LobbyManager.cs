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
        if (playButton != null)
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
    }

    public void GameStart()
    {
        StartCoroutine(xxx());
    }

    public IEnumerator xxx()
    {
        yield return LobbyConnection.Instance.StartGame();
        StartCoroutine(Utils.WaitForGameCreation());
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
            StartCoroutine(yyy());
            SceneManager.LoadScene("Araban");
        }
    }

    private IEnumerator yyy()
    {
        yield return LobbyConnection.Instance.StartGame();
    }
}
