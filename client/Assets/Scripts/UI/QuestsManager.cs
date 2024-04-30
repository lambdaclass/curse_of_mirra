using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestsManager : MonoBehaviour
{
    public Sprite claimImage;
    public Sprite claimCompleted;
    public TMP_FontAsset claimFont;
    public Sprite rerollImage;
    public List<Quest> quests;

    void Update()
    {
        foreach (Quest q in quests) 
        {
            if (q.progress == 100) 
            {
                SetReadyToClaim(q);
            }
        }
        // IsReadyToReroll()
    }

    public void SetReadyToClaim(Quest q) 
    {
        q.gameObject.transform.Find("Logo").GetComponent<Image>().sprite = claimImage;
        q.gameObject.transform.Find("Logo").GetComponent<Image>().SetNativeSize();
        q.gameObject.transform.Find("ProgressBar").Find("Completed").GetComponent<Image>().sprite = claimCompleted;
        q.gameObject.transform.Find("ProgressBar").GetComponent<Slider>().value = 1;
        q.gameObject.transform.Find("ProgressBar").Find("CompletedAmount").GetComponent<TMP_Text>().text = "READY TO CLAIM";
        q.gameObject.transform.Find("ProgressBar").Find("CompletedAmount").GetComponent<TMP_Text>().font = claimFont;
    }

    // ChangeToReroll()
    // Claim()
    // Reroll()
}
