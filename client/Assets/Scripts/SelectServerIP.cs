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

    // TODO: This should be a config file
    #if UNITY_EDITOR
        public static string serverNameString = "LOCALHOST";
        public static string serverIp = "localhost";
        public static string gatewayIp = "http://localhost:4001";
        public static string gatewayJwtPref = "GatewayJwtLocalhost";
    #else
        public static string serverNameString = "BRAZIL";
        public static string serverIp = "arena-brazil-testing.curseofmirra.com";
        public static string gatewayIp = "https://central-europe-testing.curseofmirra.com";
        public static string gatewayJwtPref = "GatewayJwtBrazil";
    #endif


    public void SetServerIp(string server)
    {
        switch (server)
        {
            case "BRAZIL":
                serverNameString = "BRAZIL";
                serverIp = "arena-brazil-testing.curseofmirra.com";
                gatewayIp = "https://central-europe-testing.curseofmirra.com";
                gatewayJwtPref = "GatewayJwtBrazil";
                break;
            case "EUROPE":
                serverNameString = "EUROPE";
                serverIp = "arena-europe-testing.curseofmirra.com";
                gatewayIp = "https://central-europe-testing.curseofmirra.com";
                gatewayJwtPref = "GatewayJwtEurope";
                break;
            case "LOCALHOST":
                serverNameString = "LOCALHOST";
                serverIp = "localhost";
                gatewayIp = "http://localhost:4001";
                gatewayJwtPref = "GatewayJwtLocalhost";
                break;
            // case "CUSTOM":
            //     serverIp = IP.GetComponent<Text>().text.Trim();
            //     serverNameString = "CUSTOM";
            //     break;
            default:
                Debug.LogError("Server selection not supported: " + server);
                break;
        }

        ServerConnection.Instance.RefreshServerInfo();
    }

    public static string GetServerIp()
    {
        return serverIp;
    }

    public static string GetGatewayIp()
    {
        return gatewayIp;
    }

    public static string GetServerName()
    {
        return serverNameString;
    }

    public static string GetGatewayJwtPref()
    {
        return gatewayJwtPref;
    }
}
