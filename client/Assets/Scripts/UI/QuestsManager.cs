using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class QuestsManager : MonoBehaviour
{
    public Sprite rerollImage;
    public List<Quest> quests;
    public TextMeshProUGUI rerollAvailable;
    public Sprite glowContainer;

    void Start()
    {
        int rerollAmount = GetRerollAmount();
        rerollAvailable.text = rerollAmount + "/6 reroll available";
    }

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

    public void ShowOriginal() 
    {
        foreach (Quest q in quests) 
        {
            q.gameObject.transform.Find("Logo").GetComponent<Image>().sprite = q.logo;
            q.gameObject.transform.Find("Logo").GetComponent<Image>().SetNativeSize();
            q.gameObject.GetComponent<CanvasGroup>().alpha = 1f;
        }
    }

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

    // Claim()
    // Reroll()
}
