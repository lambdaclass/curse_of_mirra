using TMPro;
using UnityEngine;

public class ServersListHandler : MonoBehaviour
{
    [SerializeField]
    GameObject serverOptions;
    public string serverName;

    void Start()
    {
        ChangeSelectedServerName();
    }

    public void ChangeSelectedServerName()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = LobbyConnection.Instance.serverName;
    }

    public void ShowServerOptions()
    {
        serverOptions.SetActive(true);
    }

    public void HideServerOptions()
    {
        serverOptions.SetActive(false);
    }
}
