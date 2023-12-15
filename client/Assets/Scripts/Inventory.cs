using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using MoreMountains.TopDownEngine;

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
    public Sprite myrrasBlessing;

    [SerializeField]
    Image inventoryImage;
    Vector3 imageInitialScale;
    Coroutine pickItemAnimation,
        useItemAnimation;
    Sequence pickSequenceAnimation,
        useSequenceAnimation;

    private void Start()
    {
        imageInitialScale = inventoryImage.gameObject.transform.localScale;
    }

    IEnumerator AnimatePickItem(Coroutine useItemAnimation)
    {
        inventoryImage.transform.localScale = imageInitialScale;
        inventoryContainer.SetActive(true);
        pickSequenceAnimation = DOTween.Sequence();
        pickSequenceAnimation
            .Append(inventoryContainer.GetComponent<CanvasGroup>().DOFade(1, 0.2f))
            .Insert(0.1f, inventoryImage.GetComponent<CanvasGroup>().DOFade(1, 0.3f))
            .Append(
                inventoryImage.transform
                    .DOScale(imageInitialScale + new Vector3(.15f, .15f, .15f), 1)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.InOutQuad)
            );
        sparkleEffect.SetActive(true);
        yield return new WaitForSeconds(1f);
        sparkleEffect.SetActive(false);
    }

    IEnumerator AnimateUseItem(Coroutine pickItemAnimation)
    {
        pickSequenceAnimation.Pause();
        StopCoroutine(pickItemAnimation);
        useSequenceAnimation = DOTween.Sequence();
        sparkleEffect.SetActive(true);
        useSequenceAnimation
            .Append(inventoryImage.transform.DOScale(imageInitialScale * 3, 0.7f))
            .Insert(0, inventoryImage.GetComponent<CanvasGroup>().DOFade(0, 0.5f))
            .Insert(0, inventoryContainer.GetComponent<CanvasGroup>().DOFade(0, 0.3f))
            .Append(inventoryImage.transform.DOScale(imageInitialScale, 0));
        yield return new WaitForSeconds(0.1f);
        PlayerFeedback(player, true);

        yield return new WaitForSeconds(0.5f);
        sparkleEffect.SetActive(false);
        inventoryContainer.SetActive(false);
        inventoryImage.sprite = null;

        yield return new WaitForSeconds(1f);
        PlayerFeedback(player, false);
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
            Sprite inventorySprite = GetInventoryItemSprite();
            inventoryImage.sprite = inventorySprite;
            pickItemAnimation = StartCoroutine(AnimatePickItem(useItemAnimation));
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
        return activeItem != null && inventoryImage.sprite == null;
    }

    private bool PlayerPickedUpItem(Player player)
    {
        return player != null && player.Inventory.Count > 0;
    }

    public void UseItem()
    {
        if (PlayerPickedUpItem(player))
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
        if (activeItem != null && inventoryImage.sprite != null)
        {
            UseFeedback();
        }
    }

    private void UseFeedback()
    {
        useItemAnimation = StartCoroutine(AnimateUseItem(pickItemAnimation));
    }

    void PlayerFeedback(Player player, bool show)
    {
        GameObject playerToApplyFeedback = Utils.GetPlayer(player.Id);
        playerToApplyFeedback
            .GetComponent<CharacterFeedbacks>()
            .GetFeedback(ITEM_FEEDBACK)
            .SetActive(show);
    }
}
