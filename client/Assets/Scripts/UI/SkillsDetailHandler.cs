using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillsDetailHandler : MonoBehaviour
{
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI skillDescription;
    public Sprite selectedBorder;
    public Sprite notSelectedBorder;
    public List<SkillDescription> skillsList;
    public List<Image> bordersList;

    void Awake()
    {
        SetSkillsList();
    }

    public void SetSkillsList()
    {
        skillsList.AddRange(GetComponentsInChildren<SkillDescription>());
        foreach (Image border in transform.parent.transform.parent.GetComponentsInChildren<Image>())
        {
            if (border.GetComponent<SkillDescription>() == null)
            {
                bordersList.Add(border);
            }
        }
    }

    public void SetSkillDetaill(string setSkillName, string setSkillDescription)
    {
        skillName.text = setSkillName;
        skillDescription.text = setSkillDescription;
    }

    public void ResetSelectSkill(SkillDescription selectedSkill)
    {
        skillsList.ForEach(el =>
        {
            el.GetComponent<SkillDescription>().skillBorder.sprite = notSelectedBorder;
        });
        selectedSkill.skillBorder.sprite = selectedBorder;
    }
}
