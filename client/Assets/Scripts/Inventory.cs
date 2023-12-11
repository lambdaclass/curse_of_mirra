using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    private const string MYRRAS_BLESSING = "loot_health";
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
        Debug.Log(SocketConnectionManager.Instance.playerId);
        yield return new WaitUntil(() => Utils.GetGamePlayer(SocketConnectionManager.Instance.playerId) != null);
        player = Utils.GetGamePlayer(SocketConnectionManager.Instance.playerId);
    }
    private void Update() {
        if (!activeInventory && player.Inventory.Count > 0) {
            activeInventory = true;
            activeItem = player.Inventory[0]; // GameLoot
        }
        
        if (activeItem != null) {
            Debug.Log("Active Item: " + activeItem);
            var inventorySprite = GetInventoryItemSprite();
            inventoryButton.GetComponent<Image>().sprite = myrrasBlessing;
            inventoryButton.SetActive(true);
        }
    }

    private Sprite GetInventoryItemSprite() {
        switch (activeItem.Name) {
            case MYRRAS_BLESSING:
                Debug.Log("Myrras Blessing");
                return myrrasBlessing;
            default:
                Debug.Log("Default: " + activeItem.Name);
                return null;
        }
    }
    public void UseItem()
    {
        Debug.Log("UseItem");
    }
}
