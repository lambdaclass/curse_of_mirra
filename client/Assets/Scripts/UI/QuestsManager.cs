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

        foreach (Quest quest in quests) 
        {
            if (quest.reroll) 
            {
                quest.SetReroll();
            } else 
            {
                quest.gameObject.GetComponent<CanvasGroup>().alpha = 0.4f;
            }
        }
    }

    public void ShowOriginal() 
    {
        rerollActive = false;
        int rerollAmount = GetRerollAmount();
        rerollAvailable.text = rerollAmount + "/6 reroll available";

        foreach (Quest q in quests) 
        {
            q.SetQuestContainer();
        }
    }

    public void HandleSelection(Quest quest) {
        if (quest.reroll && rerollActive) 
        {
            quest.Reroll();
        }
        else if (quest.progress == 1f && !rerollActive) 
        {
            quest.Claim();
        }
    }

    private int GetRerollAmount()
    {
        int amount = 0;

        foreach (Quest quest in quests) 
        {
            if (quest.reroll) 
            {
                amount ++;
            }
        }

        return amount;
    } 
}
