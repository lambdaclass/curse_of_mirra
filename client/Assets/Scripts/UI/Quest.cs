using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Quest : MonoBehaviour
{
    public bool reroll; 
    public float progress;
    [SerializeField] private int reward;
    [SerializeField] private Sprite logo;
    [SerializeField] private string title;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private Image logoImage;
    [SerializeField] private Image completedImage;
    [SerializeField] private TMP_Text completedAmount;
    [SerializeField] private EventTrigger eventTrigger;
    [SerializeField] private TextMeshProUGUI totalTrophies; 
    [SerializeField] private GameObject hexagonFrame;
    [SerializeField] private GameObject glowingContainer;
    [SerializeField] Sprite completed;
    [SerializeField] Sprite claimImage;
    [SerializeField] Sprite claimCompleted;
    [SerializeField] TMP_FontAsset claimFont;
    [SerializeField] Sprite rerollImage;

    void Start()
    {
        SetQuestContainer();
    }

    public void SetQuestContainer() 
    {
        glowingContainer.SetActive(false);
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
        glowingContainer.SetActive(true);
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
