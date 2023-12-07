using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    private Player player;
    private bool activeInventory;
    private GameLoot activeItem;
    [SerializeField]
    public GameObject inventoryButton;

    [SerializeField]
    public Sprite myrrasBlessing;


    private void Awake() {
        InitializePlayer();
    }

    private IEnumerator InitializePlayer() {
        yield return new WaitUntil(() => Utils.GetGamePlayer(SocketConnectionManager.Instance.playerId) != null);
        player = Utils.GetGamePlayer(SocketConnectionManager.Instance.playerId);
    }
    private void Update() {
        // Debug.Log("SCM playerId: " + SocketConnectionManager.Instance.playerId);
        // Debug.Log("GetGamePlayer: " + Utils.GetGamePlayer(SocketConnectionManager.Instance.playerId));
        // Debug.Log(player);
        // Debug.Log("Inventory Update: " + player.Inventory.Count);
        // if (!activeInventory && player.Inventory.Count > 0) {
        //     activeInventory = true;
        //     activeItem = player.Inventory[0]; // GameLoot
        // }
        
        if (activeItem != null) {
            inventoryButton.GetComponent<Image>().sprite = myrrasBlessing;
            inventoryButton.SetActive(true);
        }
    }

    public void UseItem()
    {
        Debug.Log("UseItem");
    }
}
