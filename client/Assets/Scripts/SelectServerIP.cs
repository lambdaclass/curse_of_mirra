using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectServerIP : MonoBehaviour
{
    [SerializeField]
    GameObject IP;

    [SerializeField]
    TextMeshProUGUI serverName;

    public static string serverIp;
    public static string serverNameString;

    // TODO: This should be a config file
    private const string _defaultServerName = "BRAZIL";
    private const string _defaultServerIp = "brazil-testing.curseofmirra.com";

    public void SetServerIp()
    {
        if (IP.GetComponent<Text>() != null)
        {
            serverIp = IP.GetComponent<Text>().text.Trim();
        }
        else if (IP.GetComponent<InputField>() != null)
        {
            serverIp = IP.GetComponent<InputField>().text.Trim();
        }
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
        return string.IsNullOrEmpty(serverIp) ? _defaultServerIp : serverIp;
    }

    public static string GetServerName()
    {
        return string.IsNullOrEmpty(serverNameString) ? _defaultServerName : serverNameString;
    }
}
