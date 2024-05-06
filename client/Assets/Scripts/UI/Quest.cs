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

    Slider progressSlider;
    Image logoImage;
    Image completedImage;
    TMP_Text completedAmount;
    EventTrigger eventTrigger;

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

    [SerializeField]
    Sprite rerollImage;

    [SerializeField]
    Sprite questContainer;

    [SerializeField]
    Sprite rerollContainer;

    void Start()
    {
        progressSlider = gameObject.transform.Find("ProgressBar").GetComponent<Slider>();
        logoImage = gameObject.transform.Find("Logo").GetComponent<Image>();
        completedImage = gameObject.transform.Find("ProgressBar").Find("Completed").GetComponent<Image>();
        completedAmount = gameObject.transform.Find("ProgressBar").Find("CompletedAmount").GetComponent<TMP_Text>();
        eventTrigger = gameObject.GetComponent<EventTrigger>();

        SetQuestContainer();
        StartCoroutine(CheckReadyToClaim());
    }

    IEnumerator CheckReadyToClaim() 
    {
        yield return new WaitUntil(() => this.progress == 1f);
        SetReadyToClaim();
    }

    public void SetQuestContainer() 
    {
        gameObject.GetComponent<Image>().sprite = questContainer;
        gameObject.GetComponent<Image>().SetNativeSize();
        progressSlider.value = this.progress;
        logoImage.sprite = this.logo;
        logoImage.SetNativeSize();
        completedImage.sprite = completed;
        completedAmount.text = (progress * 100f) + "/100";
    }

    public void SetReadyToClaim() 
    {
        logoImage.sprite = claimImage;
        logoImage.SetNativeSize();
        completedImage.sprite = claimCompleted;
        completedAmount.text = "READY TO CLAIM";
        completedAmount.font = claimFont;
    }

    public void SetReroll() 
    {
        logoImage.sprite = rerollImage;
        logoImage.SetNativeSize();
    }

    public void Reroll() 
    {
        // In future iterations (when having the backend) we should show a new quest but for now 
        // we will just show the same but as if not started. 
        this.progress = 0f; 
        SetQuestContainer();
        gameObject.GetComponent<Image>().sprite = rerollContainer;
        gameObject.GetComponent<Image>().SetNativeSize();
    }

    public void Claim() 
    {
        // In future iterations (when having the backend) we should show a new quest but for now 
        // we will just show the same but as if not started.
        this.progress = 0f; 
        totalTrophies.text = int.Parse(totalTrophies.text) + this.reward + "";
        SetQuestContainer();
    }
}
