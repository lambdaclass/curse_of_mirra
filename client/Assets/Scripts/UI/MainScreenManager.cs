using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainScreenManager : MonoBehaviour
{
    [SerializeField]
    UIModelManager modelManager;

    [SerializeField]
    GameObject lobbyConnectionPrefab;

    void Start()
    {
        // if (LobbyConnection.Instance == null)
        // {
        //     Instantiate(lobbyConnectionPrefab);
        // }
        modelManager.SetModel();
    }
}
