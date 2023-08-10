using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Errors : MonoBehaviour
{
    [SerializeField]
    public GameObject networkContainer;

    [SerializeField]
    public GameObject reconnectContainer;

    [SerializeField]
    public TextMeshProUGUI networkError;

    [SerializeField]
    public TextMeshProUGUI networkDescription;

    [SerializeField]
    public TextMeshProUGUI reconnectError;

    [SerializeField]
    public TextMeshProUGUI reconnectDescription;

    string ongoingGameTitle = "You have a game in progress";
    string ongoingGameDescription = "Do you want to reconnect to the game?";
    string connectionTitle = "Error";
    string connectionDescription = "Your connection to the server has been lost.";

    void Update()
    {
        if (LobbyConnection.Instance.errorConnection)
        {
            networkContainer.SetActive(true);
            HandleError();
        }
        if (LobbyConnection.Instance.errorOngoingGame)
        {
            reconnectContainer.SetActive(true);
            HandleError();
        }
    }

    public void HandleError()
    {
        if (LobbyConnection.Instance.errorConnection)
        {
            networkError.text = connectionTitle;
            networkDescription.text = connectionDescription;
        }
        if (LobbyConnection.Instance.errorOngoingGame)
        {
            reconnectError.text = ongoingGameTitle;
            reconnectDescription.text = ongoingGameDescription;
        }
    }

    public void HideConnectionError()
    {
        networkContainer.SetActive(false);
        LobbyConnection.Instance.errorConnection = false;
    }

    public void HideOngoingGameError()
    {
        reconnectContainer.SetActive(false);
        LobbyConnection.Instance.errorOngoingGame = false;
    }
}
