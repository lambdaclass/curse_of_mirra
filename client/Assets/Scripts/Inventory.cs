using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class Inventory : MonoBehaviour
{
    private const string MYRRAS_BLESSING = "loot_health";
    private Player player;
    private GameLoot activeItem;

    [SerializeField]
    public GameObject inventoryContainer;

    [SerializeField]
    public Sprite emptyInventory,
        myrrasBlessing;

    [SerializeField]
    Image inventoryImage;
    Vector3 initialScale;
    Coroutine itemAnimation;

    private void Start()
    {
        initialScale = inventoryImage.gameObject.transform.localScale;
    }

    IEnumerator AnimateItem()
    {
        yield return new WaitForSeconds(0.5f);
        inventoryImage.GetComponent<CanvasGroup>().DOFade(1, 1f);
        inventoryImage.gameObject.transform
            .DOScale(initialScale + new Vector3(0.2f, 0.2f, 0.2f), 1)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutQuad);
    }

    private void Update()
    {
        player = Utils.GetGamePlayer(SocketConnectionManager.Instance.playerId);
        if (!inventoryContainer.activeSelf && player != null && player.Inventory.Count > 0)
        {
            inventoryContainer.SetActive(true);
            activeItem = player.Inventory[0];
        }
        else if (inventoryContainer.activeSelf && player != null && player.Inventory.Count == 0)
        {
            activeItem = null;
            inventoryContainer.SetActive(false);
            inventoryImage.sprite = emptyInventory;
            if (itemAnimation != null)
            {
                StopCoroutine(itemAnimation);
            }
        }

        if (ShouldChangeInventorySprite())
        {
            Sprite inventorySprite = GetInventoryItemSprite();
            inventoryContainer.SetActive(true);
            inventoryImage.sprite = inventorySprite;
            itemAnimation = StartCoroutine(AnimateItem());
        }
    }

    private Sprite GetInventoryItemSprite()
    {
        switch (activeItem.Name)
        {
            case MYRRAS_BLESSING:
                return myrrasBlessing;
            default:
                return null;
        }
    }

    private bool ShouldChangeInventorySprite()
    {
        return activeItem != null
            && inventoryContainer.activeSelf
            && inventoryImage.sprite == emptyInventory;
    }

    public void UseItem()
    {
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        UseInventory useInventoryAction = new UseInventory { InventoryAt = 0 };
        GameAction gameAction = new GameAction
        {
            UseInventory = useInventoryAction,
            Timestamp = timestamp
        };
        SocketConnectionManager.Instance.SendGameAction(gameAction);
    }
}
