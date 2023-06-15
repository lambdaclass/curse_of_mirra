using System.Collections;
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
        StartCoroutine(WaitForLobbyCreation());
    }

    public void ConnectToLobby()
    {
        LobbyConnection.Instance.ConnectToLobby(sessionId.text);
        SceneManager.LoadScene("Lobby");
    }

    public void Back()
    {
        LobbyConnection.Instance.Init();
        SceneManager.LoadScene("Lobbies");
    }

    public void Refresh()
    {
        LobbyConnection.Instance.Refresh();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
                && LobbyConnection.Instance.playerId != -1
        );
        SceneManager.LoadScene("Lobby");
    }
}
