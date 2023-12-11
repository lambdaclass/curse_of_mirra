using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    private const string MYRRAS_BLESSING = "loot_health";
    private Player player;
    private GameLoot activeItem;

    [SerializeField]
    public GameObject inventoryButton;
    [SerializeField]
    public Sprite emptyInventory;
    [SerializeField]
    public Sprite myrrasBlessing;
    Image inventoryButtonImage;

    private void Start() {
        inventoryButtonImage = inventoryButton.GetComponent<Image>();
    }
    
    private void Update() {
        player = Utils.GetGamePlayer(SocketConnectionManager.Instance.playerId);
        if (!inventoryButton.activeSelf && player != null && player.Inventory.Count > 0) {
            inventoryButton.SetActive(true);
            activeItem = player.Inventory[0];
        } else if (inventoryButton.activeSelf && player != null && player.Inventory.Count == 0){
            activeItem = null;
            inventoryButton.SetActive(false);
            inventoryButtonImage.sprite = emptyInventory;
        }

        if (ShouldChangeInventorySprite()) {
            Sprite inventorySprite = GetInventoryItemSprite();
            inventoryButton.SetActive(true);
            inventoryButtonImage.sprite = inventorySprite;
        }
    }

    private Sprite GetInventoryItemSprite() {
        switch (activeItem.Name) {
            case MYRRAS_BLESSING:
                return myrrasBlessing;
            default:
                return null;
        }
    }

    private bool ShouldChangeInventorySprite() {
        return activeItem != null && inventoryButton.activeSelf && inventoryButtonImage.sprite == emptyInventory;
    }

    public void UseItem()
    {
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        UseInventory useInventoryAction = new UseInventory
        {
            InventoryAt = 0
        };
        GameAction gameAction = new GameAction { UseInventory = useInventoryAction, Timestamp = timestamp };
        SocketConnectionManager.Instance.SendGameAction(gameAction);
    }
}
