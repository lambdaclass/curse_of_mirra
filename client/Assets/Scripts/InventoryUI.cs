using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    private const string MYRRAS_BLESSING = "loot_health";

    // This is the base duration
    // The animations use this as a base for all the animation durations
    // If the goal is to change an entire animation duration, this is the value to change
    const float BASE_DURATION = 0.5f;
    private Entity playerEntity;
    private Item activeItem;

    [SerializeField]
    public GameObject inventoryContainer,
        sparkleEffect;

    [SerializeField]
    public Sprite myrrasBlessing;

    [SerializeField]
    Image inventoryImage;
    Vector3 imageInitialScale,
        scaleVariationForPulseAnimation;
    Coroutine pickItemAnimation,
        useItemAnimation;
    Sequence pickSequenceAnimation,
        useSequenceAnimation;

    private void Start()
    {
        imageInitialScale = inventoryImage.gameObject.transform.localScale;
        scaleVariationForPulseAnimation = new Vector3(.15f, .15f, .15f);
    }

    IEnumerator AnimatePickItem(Coroutine useItemAnimation)
    {
        inventoryImage.transform.localScale = imageInitialScale;
        inventoryContainer.SetActive(true);
        pickSequenceAnimation = DOTween.Sequence();
        pickSequenceAnimation
            .Append(inventoryContainer.GetComponent<CanvasGroup>().DOFade(1, BASE_DURATION - 0.3f))
            .Insert(
                0.1f,
                inventoryImage.GetComponent<CanvasGroup>().DOFade(1, BASE_DURATION - 0.2f)
            )
            .Append(
                inventoryImage
                    .transform
                    .DOScale(
                        imageInitialScale + scaleVariationForPulseAnimation,
                        BASE_DURATION + 0.5f
                    )
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
            .Append(inventoryImage.transform.DOScale(imageInitialScale * 3, BASE_DURATION + 0.2f))
            .Insert(0, inventoryImage.GetComponent<CanvasGroup>().DOFade(0, BASE_DURATION))
            .Insert(
                0,
                inventoryContainer.GetComponent<CanvasGroup>().DOFade(0, BASE_DURATION - 0.2f)
            )
            .Append(inventoryImage.transform.DOScale(imageInitialScale, 0));
        yield return new WaitForSeconds(0.1f);
        PlayerFeedback(playerEntity, true);

        yield return new WaitForSeconds(BASE_DURATION);
        sparkleEffect.SetActive(false);
        inventoryContainer.SetActive(false);
        inventoryImage.sprite = null;

        yield return new WaitForSeconds(1f);
        PlayerFeedback(playerEntity, false);
    }

    private void Update()
    {
        playerEntity = Utils.GetGamePlayer(GameServerConnectionManager.Instance.playerId);
        if (PlayerHasItem(playerEntity))
        {
            activeItem = playerEntity.Player.Inventory;
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

    private bool PlayerHasItem(Entity playerEntity)
    {
        return playerEntity != null && playerEntity.Player.Inventory != null;
    }

    public void UseItem()
    {
        if (PlayerHasItem(playerEntity))
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            GameServerConnectionManager.Instance.SendUseItem(timestamp);
        }
        if (activeItem != null && inventoryImage.sprite != null)
        {
            useItemAnimation = StartCoroutine(AnimateUseItem(pickItemAnimation));
        }
    }

    void PlayerFeedback(Entity player, bool state)
    {
        GameObject playerToExecuteFeedback = Utils.GetPlayer(player.Id);
        playerToExecuteFeedback.GetComponent<CharacterInventory>().ExecuteFeedback(state);
    }
}
