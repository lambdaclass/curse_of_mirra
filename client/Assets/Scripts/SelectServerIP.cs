using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectServerIP : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI IP;

    [SerializeField]
    TextMeshProUGUI severName;

    public static string serverIp = "localhost";
    public static string serverNameString = "LocalHost";

    public void SetServerIp()
    {
        serverIp = IP.text;
        serverNameString = severName.text;
        LobbyConnection.Instance.Refresh();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static string GetServerIp()
    {
        Debug.Log("server ip: " + serverIp);
        return string.IsNullOrEmpty(serverIp) ? "localhost" : serverIp;
    }

    public static string GetServerName()
    {
        Debug.Log("server name: " + serverNameString);
        return string.IsNullOrEmpty(serverNameString) ? "LocalHost" : serverNameString;
    }
}
