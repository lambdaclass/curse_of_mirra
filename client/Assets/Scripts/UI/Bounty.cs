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

    public void SetBountyContainer(BountyInfo bounty) 
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
}
