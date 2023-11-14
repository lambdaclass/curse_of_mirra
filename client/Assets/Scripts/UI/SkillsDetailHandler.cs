using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SkillsDetailHandler : MonoBehaviour
{
    public TextMeshProUGUI skillSetType;
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

    public void SetSkillDetaill(
        string setSkillType,
        string setSkillName,
        string setSkillDescription
    )
    {
        skillSetType.text = setSkillType + ": ";
        skillName.text = setSkillName;
        skillDescription.text = setSkillDescription;
    }

    public void ResetSelectSkill(SkillDescription selectedSkill)
    {
        skillsList.ForEach(el =>
        {
            el.GetComponent<SkillDescription>().skillBorder.sprite = notSelectedBorder;
            el.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f).SetEase(Ease.InQuad);
        });
        selectedSkill.skillBorder.sprite = selectedBorder;
        selectedSkill.transform.DOScale(new Vector3(1.3f, 1.3f, 1.3f), 0.2f).SetEase(Ease.OutQuad);
    }
}
