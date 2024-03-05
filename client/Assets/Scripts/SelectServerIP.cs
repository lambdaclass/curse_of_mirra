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
    private const string _defaultServerName = "BRAZIL";
    private const string _defaultServerIp = "brazil-testing.curseofmirra.com";

    public void SetServerIp()
    {
        serverIp = IP.text.Trim();
        if (serverName.text.ToUpper() == "SET CUSTOM")
        {
            serverNameString = "CUSTOM";
        }
        else
        {
            serverNameString = serverName.text;
        }
        ServerConnection.Instance.RefreshServerInfo();
    }

    public static string GetServerIp()
    {
        print(serverIp);
        return string.IsNullOrEmpty(serverIp) ? _defaultServerIp : serverIp;
    }

    public static string GetServerName()
    {
        return string.IsNullOrEmpty(serverNameString) ? _defaultServerName : serverNameString;
    }
}
