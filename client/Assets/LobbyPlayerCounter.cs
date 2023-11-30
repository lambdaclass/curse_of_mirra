using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayerCounter : MonoBehaviour
{

    public float updateInterval = 5f;
    protected float _timeLeft;
    protected Text _text;
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Text>() == null)
        {
            Debug.LogWarning("PlayerCounter requires a GUIText component.");
            return;
        }
        _text = GetComponent<Text>();
        _timeLeft = updateInterval;
    }

    // Update is called once per frame
    void Update()
    {
         _timeLeft = _timeLeft - Time.deltaTime;
        if (_timeLeft <= 0.0)
        {
            _timeLeft = updateInterval;
            _text.text = LobbyConnection.Instance.playerCount.ToString() + " / "+ LobbyConnection.Instance.lobbyCapacity.ToString() + " players";
        }
    }
}
