using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using MoreMountains.TopDownEngine;

public class Utils
{
    public static IEnumerator WaitForGameCreation(string levelName)
    {
        yield return new WaitUntil(
            () => !string.IsNullOrEmpty(LobbyConnection.Instance.GameSession)
        );
        SceneManager.LoadScene(levelName);
    }

    public static Vector3 transformBackendPositionToFrontendPosition(Position position)
    {
        var x = (long)position.Y / 10f - 50.0f;
        var y = (-((long)position.X)) / 10f + 50.0f;
        return new Vector3(x, 1f, y);
    }

    public static GameObject GetPlayer(int id)
    {
        return SocketConnectionManager.Instance.players.Find(
            el => el.GetComponent<Character>().PlayerID == id.ToString()
        );
    }
}
