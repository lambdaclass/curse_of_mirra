using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class Inventory : MonoBehaviour
{
    private const string MYRRAS_BLESSING = "loot_health";
    private const string ITEM_FEEDBACK = "UseItemEffect";
    private Player player;
    private GameLoot activeItem;

    [SerializeField]
    public GameObject inventoryContainer,
        sparkleEffect;

    [SerializeField]
    public Sprite emptyInventory,
        myrrasBlessing;

    [SerializeField]
    Image inventoryImage;
    Vector3 imageInitialScale;
    Coroutine pickItemAnimation,
        useItemAnimation;

    private void Start()
    {
        imageInitialScale = inventoryImage.gameObject.transform.localScale;
    }

    IEnumerator AnimatePickItem()
    {
        inventoryContainer.SetActive(true);
        inventoryContainer.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
        sparkleEffect.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        inventoryImage.GetComponent<CanvasGroup>().DOFade(1, 1f);
        inventoryImage.gameObject.transform
            .DOScale(imageInitialScale + new Vector3(0.2f, 0.2f, 0.2f), 1)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(0.2f);
        sparkleEffect.SetActive(false);
    }

    IEnumerator AnimateUseItem()
    {
        inventoryImage.GetComponent<CanvasGroup>().DOFade(0, 1f);
        Sequence iconSequence = DOTween.Sequence();
        iconSequence
            .Append(
                inventoryImage.gameObject.transform.DOScale(
                    imageInitialScale + new Vector3(0.2f, 0.2f, 0.2f),
                    .5f
                )
            )
            .Append(
                inventoryImage.gameObject.transform.DOScale(
                    imageInitialScale + new Vector3(0.2f, 0.2f, 0.2f),
                    .5f
                )
            )
            .SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(0.2f);
        sparkleEffect.SetActive(true);
        inventoryContainer.GetComponent<CanvasGroup>().DOFade(1, 1f);
        yield return new WaitForSeconds(1f);
        sparkleEffect.SetActive(false);
        inventoryContainer.SetActive(false);
        StopCoroutine(useItemAnimation);
    }

    private void Update()
    {
        player = Utils.GetGamePlayer(SocketConnectionManager.Instance.playerId);
        if (PlayerPickedUpItem(player))
        {
            activeItem = player.Inventory[0];
        }
        else
        {
            activeItem = null;
        }
        if (ShouldChangeInventorySprite())
        {
            PlayerFeedback(player);
            Sprite inventorySprite = GetInventoryItemSprite();
            inventoryImage.sprite = inventorySprite;
            pickItemAnimation = StartCoroutine(AnimatePickItem());
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
        return activeItem != null;
    }

    private bool PlayerPickedUpItem(Player player)
    {
        return player != null && player.Inventory.Count > 0;
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
        UseFeedback();
    }

    private void UseFeedback()
    {
        if (pickItemAnimation != null)
        {
            StopCoroutine(pickItemAnimation);
        }
        useItemAnimation = StartCoroutine(AnimateUseItem());
    }

    void PlayerFeedback(Player player)
    {
        GameObject playerToApplyFeedback = Utils.GetPlayer(player.Id);
        playerToApplyFeedback
            .GetComponent<CharacterFeedbacks>()
            .GetFeedback(ITEM_FEEDBACK)
            .SetActive(true);
    }
}
