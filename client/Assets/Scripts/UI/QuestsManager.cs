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
    private bool rerollActive = false;

    void Start()
    {
        int rerollAmount = GetRerollAmount();
        rerollAvailable.text = rerollAmount + "/6 reroll available";
    }

    public void ShowReroll() 
    {
        rerollActive = true;

        foreach (Quest q in quests) 
        {
            if (q.reroll) 
            {
                q.ChangeToReroll();
            } else 
            {
                q.SetAsInactive();
            }
        }
    }

    public void ShowOriginal() 
    {
        foreach (Quest q in quests) 
        {
            q.SetQuestContainer();
            q.gameObject.GetComponent<CanvasGroup>().alpha = 1f;
        }
    }

    public void HandleSelection(Quest q) {
        if (q.reroll && rerollActive) 
        {
            q.Reroll();
        }
        else if (q.progress == 1f && !rerollActive) 
        {
            q.Claim();
        }
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
}
