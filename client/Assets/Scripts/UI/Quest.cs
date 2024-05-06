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
        progressSlider = gameObject.transform.Find("ProgressBar").GetComponent<Slider>();
        logoImage = gameObject.transform.Find("Logo").GetComponent<Image>();
        completedImage = gameObject.transform.Find("ProgressBar").Find("Completed").GetComponent<Image>();
        completedAmount = gameObject.transform.Find("ProgressBar").Find("CompletedAmount").GetComponent<TMP_Text>();

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
        progressSlider.value = this.progress;
        logoImage.sprite = this.logo;
        logoImage.SetNativeSize();
        completedImage.sprite = completed;
        completedAmount.text = (progress * 100f) + "/100";
    }

    private void SetReadyToClaim() 
    {
        logoImage.sprite = claimImage;
        logoImage.SetNativeSize();
        completedImage.sprite = claimCompleted;
        completedAmount.text = "READY TO CLAIM";
        completedAmount.font = claimFont;
        gameObject.GetComponent<EventTrigger>().enabled = true;
    }
}
