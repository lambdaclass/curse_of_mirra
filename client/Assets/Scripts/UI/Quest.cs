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

    void Start()
    {
        SetQuestContainer();
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
}
