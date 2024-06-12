using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class Bounty : MonoBehaviour
{
    BountyInfo bountyInfo;

    [SerializeField] 
    private TMP_Text descriptionText,
        rewardText;

    [SerializeField] 
    private Image currency,
        icon;

    [SerializeField] 
    private GameObject bountyContainer;

    [SerializeField] RectTransform rectTransform;
    [SerializeField] Sprite goldImage;
    [SerializeField] Sprite gemImage;
    [SerializeField] Sprite killImage;
    [SerializeField] Sprite powerUpImage;
   
    public void SetBounty(BountyInfo bounty) 
    {
        // For now we need to handle that no bounty is selected.
        // This will be added on a future iteration.
        if (bounty == null) 
        {
            bountyContainer.SetActive(false);
        } else {
            bountyInfo = bounty;
            descriptionText.text = bounty.Description;
            rewardText.text = bounty.Reward.Amount.ToString();

            switch (bounty.Reward.Currency)
            {
                case "Gold":
                    currency.sprite = goldImage;
                    break;
                default:
                    currency.sprite = gemImage;
                    break;
            }

            switch (bounty.QuestType)
            {
                case "kills":
                    icon.sprite = killImage;
                    break;
                default:
                    icon.sprite = powerUpImage;
                    break;
            }
        }
    }

    public void SelectBounty() 
    {
        float moveDuration = .3f; 
        float bounceDuration = .5f; 
        float bounceScale = 1.12f; 

        GameServerConnectionManager.Instance.SendSelectBounty(bountyInfo.Id);
        GameServerConnectionManager.Instance.bountySelected = bountyInfo;

        Vector2 targetPosition = new Vector2(0, rectTransform.anchoredPosition.y); 
        rectTransform.DOAnchorPos(targetPosition, moveDuration)
            .OnComplete(() =>
            {
                rectTransform.DOScale(bounceScale, bounceDuration)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.InOutQuad);
            });
    } 
}
