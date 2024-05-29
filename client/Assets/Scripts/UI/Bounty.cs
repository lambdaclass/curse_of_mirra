using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounty : MonoBehaviour
{
    private string id;

    [SerializeField] private 
    TMP_Text description,
        reward;

    [SerializeField] private Sprite currency;
    [SerializeField] private int reward;
    [SerializeField] Sprite goldImage;
    [SerializeField] Sprite gemImage;

    void SetBountyContainer(BountyInfo bounty) 
    {
        id = bounty.Id;
        description = bounty.Description;
        reward = bounty.Reward.Amount;

        switch (bounty.Reward.Currency)
        {
            case "Gold":
                currency = goldImage;
                break;
            default:
                currency = gemImage;
                break;
        }
    }
}
