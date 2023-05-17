using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : LevelSelector
{
    public override void GoToLevel()
    {
        base.GoToLevel();
        if (SceneManager.GetActiveScene().name == "Lobbies")
        {
            LobbyConnection.Instance.CreateLobby();
        }
        else
        {
            LobbyConnection.Instance.StartGame();
        }
        gameObject.GetComponent<MMTouchButton>().DisableButton();
    }

    void update()
    {
        if (!String.IsNullOrEmpty(LobbyConnection.Instance.GameSession))
        {
            GoToLevel();
        }
    }
}
