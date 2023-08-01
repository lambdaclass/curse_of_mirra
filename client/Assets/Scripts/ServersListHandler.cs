using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        GetComponentInChildren<TextMeshProUGUI>().text = LobbyConnection.Instance.server_name;
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
