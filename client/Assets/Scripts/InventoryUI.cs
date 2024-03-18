using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    // This is the base duration
    // The animations use this as a base for all the animation durations
    // If the goal is to change an entire animation duration, this is the value to change
    const float BASE_DURATION = 0.5f;
    private Entity playerEntity;
    private CharacterFeedbacks characterFeedbacks;
    private Item activeItem;

    [SerializeField]
    public GameObject inventoryContainer,
        sparkleEffect;

    [SerializeField]
    public Sprite myrrasBlessing;
    public Sprite goldenClock;
    public Sprite magicBoots;

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
        HandlePlayerUseItemFeedback(true);

        yield return new WaitForSeconds(BASE_DURATION);
        sparkleEffect.SetActive(false);
        inventoryContainer.SetActive(false);
        inventoryImage.sprite = null;

        yield return new WaitForSeconds(1f);
        HandlePlayerUseItemFeedback(false);
    }

    private void Update()
    {
        if (GameServerConnectionManager.Instance.players.Count > 0 && characterFeedbacks == null)
        {
            characterFeedbacks = Utils
                .GetCharacter(GameServerConnectionManager.Instance.playerId)
                .GetComponent<CharacterFeedbacks>();
        }
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
        // TODO: Change to List
        switch (activeItem.Name)
        {
            case "mirra_blessing":
                return myrrasBlessing;
            case "golden_clock":
                return goldenClock;
            case "magic_boots":
                return magicBoots;
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

    void HandlePlayerUseItemFeedback(bool state)
    {
        characterFeedbacks.ExecuteUseItemFeedback(state);
    }
}
