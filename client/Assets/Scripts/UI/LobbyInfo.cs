using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class LobbyInfo : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI lobbyID;

    [SerializeField]
    TextMeshProUGUI playersAmount;

    [SerializeField]
    TextMeshProUGUI mapName;

    [SerializeField]
    private AssetReference reference;

    // Start is called before the first frame update
    void Start()
    {
        var sprite = Addressables.LoadAssetAsync<Sprite>(reference).Result;
        string id = LobbyConnection.Instance.LobbySession.ToString();
        lobbyID.text = "# " + id.Substring(id.Length - 5);

        // TODO bring from LobbyConnection map name
        mapName.text = "Araban";
    }

    // Update is called once per frame
    void Update()
    {
        playersAmount.text = LobbyConnection.Instance.playerCount.ToString();
    }
}
