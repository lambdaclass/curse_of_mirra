using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Quest : MonoBehaviour
{
    public int reward;
    public Sprite logo;
    public string title;
    public float progress;
    public bool reroll; 

    [SerializeField] 
    TextMeshProUGUI totalTrophies; 

    [SerializeField] 
    Sprite completed;

    [SerializeField]  
    Sprite claimImage;

    [SerializeField]  
    Sprite claimCompleted;

    [SerializeField]  
    TMP_FontAsset claimFont;

    void Start()
    {
        SetQuestContainer();
        StartCoroutine(CheckReadyToClaim());
    }

    IEnumerator CheckReadyToClaim() 
    {
        yield return new WaitUntil(() => this.progress == 1f);
        SetReadyToClaim();
    }

    public void SelectQuest() 
    {
        if (this.progress == 1f) 
        {
            this.progress = 0f; 
            totalTrophies.text = int.Parse(totalTrophies.text) + this.reward + "";
            SetQuestContainer();
        }
    }

    private void SetQuestContainer() 
    {
        gameObject.GetComponent<EventTrigger>().enabled = false;
        gameObject.transform.Find("ProgressBar").GetComponent<Slider>().value = this.progress;
        gameObject.transform.Find("Logo").GetComponent<Image>().sprite = this.logo;
        gameObject.transform.Find("Logo").GetComponent<Image>().SetNativeSize();
        gameObject.transform.Find("ProgressBar").Find("Completed").GetComponent<Image>().sprite = completed;
        gameObject.transform.Find("ProgressBar").Find("CompletedAmount").GetComponent<TMP_Text>().text = (progress * 100f) + "/100";
    }

    private void SetReadyToClaim() 
    {
        gameObject.transform.Find("Logo").GetComponent<Image>().sprite = claimImage;
        gameObject.transform.Find("Logo").GetComponent<Image>().SetNativeSize();
        gameObject.transform.Find("ProgressBar").Find("Completed").GetComponent<Image>().sprite = claimCompleted;
        gameObject.transform.Find("ProgressBar").Find("CompletedAmount").GetComponent<TMP_Text>().text = "READY TO CLAIM";
        gameObject.transform.Find("ProgressBar").Find("CompletedAmount").GetComponent<TMP_Text>().font = claimFont;
        gameObject.GetComponent<EventTrigger>().enabled = true;
    }
}
