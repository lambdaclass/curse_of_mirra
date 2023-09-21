using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillsDetailHandler : MonoBehaviour
{
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI skillDescription;
    public List<SkillDescription> list;

    void Awake()
    {
        SetSkillsList();
    }

    public void SetSkillsList()
    {
        list.AddRange(GetComponentsInChildren<SkillDescription>());
    }

    public void SetSkillDetaill(string setSkillName, string setSkillDescription)
    {
        skillName.text = setSkillName;
        skillDescription.text = setSkillDescription;
    }

    public void ResetSelectSkill(Sprite skillIcon, Color color)
    {
        list.ForEach(el =>
        {
            el.GetComponent<Image>().color = color;
        });
    }
}
