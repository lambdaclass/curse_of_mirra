using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbiesManager : LevelSelector
{
    [SerializeField]
    Text sessionId;


    public override void GoToLevel()
    {
        base.GoToLevel();
        gameObject.GetComponent<MMTouchButton>().DisableButton();
    }

    public void CreateLobby()
    {
        LobbyConnection.Instance.CreateLobby();
        GoToLevel();
    }

    public void ConnectToLobby()
    {
        LobbyConnection.Instance.ConnectToLobby(sessionId.text);
        GoToLevel();
    }

    public void Back()
    {
        LobbyConnection.Instance.Init();
        GoToLevel();
    }

    public void Refresh()
    {
        LobbyConnection.Instance.Refresh();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuickGame()
    {
        LobbyConnection.Instance.QuickGame();
        GoToLevel();
    }
}
