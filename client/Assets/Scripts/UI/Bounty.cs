using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Bounty : MonoBehaviour
{
    private string id;

    [SerializeField] private 
    TMP_Text descriptionText,
        rewardText;

    [SerializeField] private 
    Image currency,
        icon;

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
        GameServerConnectionManager.Instance.SendSelectBounty(id);

        // // Calculate the center position of the screen in world coordinates
        // Vector2 centerPosition = centralBountyRectTransform.anchoredPosition;

        // // Move the target object to the center position along the x-axis
        // bounty.transform.DOMoveX(centerPosition.x, moveDuration)
        //     .OnComplete(() =>
        //     {
        //         // Start bouncing when the movement to the center is complete
        //         bounty.transform.DOShakePosition(bounceDuration, new Vector3(bounceStrength, 0, 0), bounceVibrato, 90, false, false)
        //             .SetLoops(-1, LoopType.Yoyo);
        //     });
    } 
}
