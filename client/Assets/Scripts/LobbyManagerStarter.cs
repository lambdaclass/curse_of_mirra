using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManagerStarter : LevelSelector
{
    [SerializeField]
    Text sessionId;

    public override void GoToLevel()
    {

        Debug.Log(sessionId.text);
        LobbyConnection.Instance.ConnectToLobby(sessionId.text);
        base.GoToLevel();
        gameObject.GetComponent<MMTouchButton>().DisableButton();
    }
}
