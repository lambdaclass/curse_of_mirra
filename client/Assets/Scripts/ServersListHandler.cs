using TMPro;
using UnityEngine;

public class ServersListHandler : MonoBehaviour
{
    [SerializeField]
    GameObject serverOptions;

    [SerializeField]
    TextMeshProUGUI activeServerName;

    void Start()
    {
        ChangeSelectedServerName();
    }

    void Update()
    {
        if (
            activeServerName.text != LobbyConnection.Instance.serverName
            || activeServerName.text == ""
        )
        {
            ChangeSelectedServerName();
        }
    }

    public void ChangeSelectedServerName()
    {
        activeServerName.text = LobbyConnection.Instance.serverName;
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
