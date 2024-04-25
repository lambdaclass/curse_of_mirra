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
        SceneManager.LoadScene("Lobby");
    }
}
