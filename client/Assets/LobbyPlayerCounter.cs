using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayerCounter : MonoBehaviour
{
    protected TMP_Text _totalLobbyPlayersText;
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.GetComponent<TMP_Text>() == null)
        {
            Debug.LogWarning("PlayerCounter requires a GUIText component.");
            return;
        }
        _totalLobbyPlayersText = gameObject.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        var playerAmount = Math.Max(LobbyConnection.Instance.playerCount, LobbyConnection.Instance.simulatedPlayerCount);
        _totalLobbyPlayersText.text = playerAmount.ToString() + " / " + LobbyConnection.Instance.lobbyCapacity.ToString();
    }
}
