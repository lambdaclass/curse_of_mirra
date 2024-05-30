using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class Bounty : MonoBehaviour
{
    [SerializeField] private 
    string id;

    [SerializeField] private 
    TMP_Text descriptionText,
        rewardText;

    [SerializeField] private 
    Image currency,
        icon;

    [SerializeField] RectTransform rectTransform;
    [SerializeField] Sprite goldImage;
    [SerializeField] Sprite gemImage;
    [SerializeField] Sprite killImage;
    [SerializeField] Sprite powerUpImage;
   
    public void SetBounty(BountyInfo bounty) 
    {
        id = bounty.Id;
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

        // switch (bounty.Type)
        // {
        //     case "Kill":
        //         icon.sprite = killImage;
        //         break;
        //     case "PowerUp":
        //         icon.sprite = powerUpImage;
        //         break;
        // }
    }

    public void SelectBounty() 
    {
        float moveDuration = .3f; 
        float bounceDuration = .5f; 
        float bounceScale = 1.12f; 

        GameServerConnectionManager.Instance.SendSelectBounty(id);

        Vector2 targetPosition = new Vector2(0, rectTransform.anchoredPosition.y); 

        rectTransform.DOScale(.8f, .5f);
        rectTransform.DOAnchorPos(targetPosition, moveDuration)
            .OnComplete(() =>
            {
                rectTransform.DOScale(bounceScale, bounceDuration)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.InOutQuad);
            });
    } 
}
