using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayerCounter : MonoBehaviour
{

    public float updateInterval = 5f;
    protected float _timeLeftToUpdate;
    protected TMP_Text _totalLobbyPlayersText;
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Text>() == null)
        {
            Debug.LogWarning("PlayerCounter requires a GUIText component.");
            return;
        }
        _timeLeftToUpdate = updateInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if (_totalLobbyPlayersText == null) {
            _totalLobbyPlayersText = gameObject.GetComponent<TMP_Text>();
        }

         _timeLeftToUpdate = _timeLeftToUpdate - Time.deltaTime;
        if (_timeLeftToUpdate <= 0.0)
        {
            _timeLeftToUpdate = updateInterval;
            _totalLobbyPlayersText.text = LobbyConnection.Instance.playerCount.ToString() + " / " + LobbyConnection.Instance.lobbyCapacity.ToString();
        }
    }
}
