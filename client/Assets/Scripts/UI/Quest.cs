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
    public Slider progressSlider;
    public Image logoImage;
    public Image completedImage;
    public TMP_Text completedAmount;
    public EventTrigger eventTrigger;
    public TextMeshProUGUI totalTrophies; 
    public GameObject hexagonFrame;

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
        SetQuestContainer();
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
        hexagonFrame.SetActive(false);
        gameObject.GetComponent<CanvasGroup>().alpha = 1f;

        // Recreate the ready to claim check
        // (This should actually be done on the backend)
        if (this.progress == 1f) 
        {
            SetReadyToClaim();
        }
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
        this.reroll = false;
        SetQuestContainer();
        gameObject.GetComponent<Image>().sprite = rerollContainer;
        gameObject.GetComponent<Image>().SetNativeSize();
        hexagonFrame.SetActive(true);
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
