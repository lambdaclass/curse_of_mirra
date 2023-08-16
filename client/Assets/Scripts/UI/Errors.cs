using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

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

    [SerializeField]
    public MMTouchButton yesButton;

    public static Errors Instance;

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(transform.parent.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(transform.parent.gameObject);

        UnityEvent reconnectEvent = new UnityEvent();
        reconnectEvent.AddListener(Reconnect);
        yesButton.ButtonPressedFirstTime = reconnectEvent;
    }

    public void Reconnect()
    {
        // TODO: This is what LobbiesManager.Reconnect() does
        // whe should leave this here or instantiate that
        LobbyConnection.Instance.Reconnect();
        // TODO: it should go directly to the Battle if I want to go back to it
        SceneManager.LoadScene("CharacterSelection");
        HideOngoingGameError();
    }

    public void HandleReconnectError(string title, string description)
    {
        reconnectContainer.SetActive(true);
        reconnectError.text = title;
        reconnectDescription.text = description;
    }

    public void HideOngoingGameError()
    {
        reconnectContainer.SetActive(false);
    }

    public void HandleNetworkError(string title, string description)
    {
        networkContainer.SetActive(true);
        networkError.text = title;
        networkDescription.text = description;
    }

    public void HideConnectionError()
    {
        networkContainer.SetActive(false);
    }
}
