using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillsDetailHandler : MonoBehaviour
{
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI skillDescription;
    public List<GameObject> list;

    void Awake()
    {
        SetSkillsList();
    }

    public void SetSkillsList()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            list.Add(transform.GetChild(i).gameObject);
        }
    }

    public void SetSkillDetaill(string setSkillName, string setSkillDescription)
    {
        skillName.text = setSkillName;
        skillDescription.text = setSkillDescription;
    }

    public void SetSkillIcon(Sprite skillIcon, Sprite selectedSkillIcon)
    {
        list.ForEach(el =>
        {
            if (
                el.GetComponent<Image>().sprite == skillIcon
                || el.GetComponent<Image>().sprite == selectedSkillIcon
            )
            {
                el.GetComponent<Image>().sprite = selectedSkillIcon;
            }
            else
            {
                el.GetComponent<Image>().sprite = el.GetComponent<SkillDescription>().skillSprite;
            }
        });
    }
}
