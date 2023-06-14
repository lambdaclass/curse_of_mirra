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

    [SerializeField] GameObject mapList;

    public static string LevelSelected;

    public override void GoToLevel()
    {
        base.GoToLevel();
        gameObject.GetComponent<MMTouchButton>().DisableButton();
    }

    void Start()
    {
        if (playButton != null && mapList != null)
        {
            if (LobbyConnection.Instance.playerId == 1)
            {
                playButton.SetActive(true);
            }
            else
            {
                playButton.SetActive(false);
                mapList.SetActive(false);
            }
        }
    }

    public void GameStart()
    {
        LobbyConnection.Instance.StartGame();
        print(this.LevelName);
        StartCoroutine(Utils.WaitForGameCreation(this.LevelName));
    }

    public void Back()
    {
        LobbyConnection.Instance.Init();
        SceneManager.LoadScene("Lobbies");
    }

    public void SelectMap(string mapName)
    {
        this.LevelName = mapName;
        LevelSelected = mapName;
    }

    private void Update()
    {
        if (
            !String.IsNullOrEmpty(LobbyConnection.Instance.GameSession)
            && !LobbyConnection.Instance.gameStarted
        )
        {
            print(this.LevelName);
            print(LevelSelected);
            LobbyConnection.Instance.StartGame();
            SceneManager.LoadScene("BackendPlayground");
        }
    }
}
