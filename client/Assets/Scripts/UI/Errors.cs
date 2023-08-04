using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Errors : MonoBehaviour
{
    [SerializeField]
    public GameObject container;

    [SerializeField]
    public TextMeshProUGUI error;

    [SerializeField]
    public TextMeshProUGUI description;

    [SerializeField]
    public GameObject yesButton;

    [SerializeField]
    public GameObject noButton;

    [SerializeField]
    public GameObject okButton;

    string ongoingGameTitle = "You have a game in progress";
    string ongoingGameDescription = "Do you want to reconnect to the game?";
    string connectionTitle = "Error";
    string connectionDescription = "Your connection to the server has been lost.";

    void Update()
    {
        if (LobbyConnection.Instance.errorConnection || LobbyConnection.Instance.errorOngoingGame)
        {
            container.SetActive(true);
            HandleError();
        }
    }

    public void HandleError()
    {
        if (LobbyConnection.Instance.errorConnection)
        {
            error.text = connectionTitle;
            description.text = connectionDescription;
            okButton.SetActive(LobbyConnection.Instance.errorConnection);
        }
        if (LobbyConnection.Instance.errorOngoingGame)
        {
            error.text = ongoingGameTitle;
            description.text = ongoingGameDescription;
            yesButton.SetActive(LobbyConnection.Instance.errorOngoingGame);
            noButton.SetActive(LobbyConnection.Instance.errorOngoingGame);
        }
    }

    public void HideConnectionError()
    {
        container.SetActive(false);
        LobbyConnection.Instance.errorConnection = false;
    }

    public void HideOngoingGameError()
    {
        container.SetActive(false);
        LobbyConnection.Instance.errorOngoingGame = false;
    }
}
