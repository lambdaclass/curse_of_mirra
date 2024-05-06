using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class QuestsManager : MonoBehaviour
{
    // public Sprite claimImage;
    // public Sprite claimCompleted;
    // public TMP_FontAsset claimFont;
    public Sprite rerollImage;
    public List<Quest> quests;
    public TextMeshProUGUI rerollAvailable;
    public Sprite glowContainer;

    void Start()
    {
        int rerollAmount = GetRerollAmount();
        rerollAvailable.text = rerollAmount + "/6 reroll available";
    }

    // void Update()
    // {
    //     foreach (Quest q in quests) 
    //     {
    //         if (q.progress == 1f) 
    //         {
    //             SetReadyToClaim(q);
    //         } 
    //     }
    //     // IsReadyToReroll()
    // }

    public void ShowReroll() 
    {
        foreach (Quest q in quests) 
        {
            if (q.reroll) 
            {
                ChangeToReroll(q);
            } else 
            {
                q.gameObject.GetComponent<CanvasGroup>().alpha = 0.4f;
                q.gameObject.GetComponent<EventTrigger>().enabled = false;
            }
        }
    }

    public void ChangeToOriginal() 
    {
        foreach (Quest q in quests) 
        {
            q.gameObject.transform.Find("Logo").GetComponent<Image>().sprite = q.logo;
            q.gameObject.transform.Find("Logo").GetComponent<Image>().SetNativeSize();
            q.gameObject.GetComponent<CanvasGroup>().alpha = 1f;
        }
    }

    // private void SetReadyToClaim(Quest q) 
    // {
    //     q.gameObject.transform.Find("Logo").GetComponent<Image>().sprite = claimImage;
    //     q.gameObject.transform.Find("Logo").GetComponent<Image>().SetNativeSize();
    //     q.gameObject.transform.Find("ProgressBar").Find("Completed").GetComponent<Image>().sprite = claimCompleted;
    //     // q.gameObject.transform.Find("ProgressBar").GetComponent<Slider>().value = 1;
    //     q.gameObject.transform.Find("ProgressBar").Find("CompletedAmount").GetComponent<TMP_Text>().text = "READY TO CLAIM";
    //     q.gameObject.transform.Find("ProgressBar").Find("CompletedAmount").GetComponent<TMP_Text>().font = claimFont;
    //     q.gameObject.GetComponent<EventTrigger>().enabled = true;
    // }

    private void ChangeToReroll(Quest q) 
    {
        q.gameObject.GetComponent<EventTrigger>().enabled = true;
        q.gameObject.transform.Find("Logo").GetComponent<Image>().sprite = rerollImage;
        q.gameObject.transform.Find("Logo").GetComponent<Image>().SetNativeSize();
    }

    private int GetRerollAmount()
    {
        int amount = 0;

        foreach (Quest q in quests) 
        {
            if (q.reroll) 
            {
                amount ++;
            }
        }

        return amount;
    }

    // public void SetAsInactive(q)
    // {
    //     null;
    // }

    // Claim()
    // Reroll()
}
