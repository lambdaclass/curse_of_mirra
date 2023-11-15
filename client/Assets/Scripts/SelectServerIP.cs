using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectServerIP : MonoBehaviour
{
    [SerializeField]
    Text IP;

    [SerializeField]
    TextMeshProUGUI serverName;

    public static string serverIp;
    public static string serverNameString;

    // TODO: This should be a config file
    private const string _defaultServerIp = "aws-testing.dark-worlds.lambdaclass.com";

    public void SetServerIp()
    {
        serverIp = IP.text.Trim();
        serverNameString = serverName.text;
        LobbyConnection.Instance.Refresh();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static string GetServerIp()
    {
        return string.IsNullOrEmpty(serverIp) ? _defaultServerIp : serverIp;
    }

    public static string GetServerName()
    {
        return string.IsNullOrEmpty(serverNameString) ? "Brazil" : serverNameString;
    }
}
