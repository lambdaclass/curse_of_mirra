using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Utils
{
    public static IEnumerator WaitForGameCreation()
    {
        yield return new WaitUntil(
            () => !string.IsNullOrEmpty(LobbyConnection.Instance.GameSession)
        );
        SceneManager.LoadScene("Araban");
    }

    public static Vector3 transformBackendPositionToFrontendPosition(Position position)
    {
        var x = (long)position.Y / 10f - 50.0f;
        var y = (-((long)position.X)) / 10f + 50.0f;
        return new Vector3(x, 1f, y);
    }
}
