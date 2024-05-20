using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillsDetailHandler : MonoBehaviour
{
    public TextMeshProUGUI skillSetType;
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI skillDescription;
    public List<SkillDescription> skillsList;
    public List<Image> bordersList;

    [SerializeField] public CharacterInfoManager characterInfoManager;

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
        skillSetType.text = setSkillType;
        skillName.text = setSkillName;
        skillDescription.text = setSkillDescription;
    }

    public void ResetSelectSkill(SkillDescription selectedSkill)
    {
        skillsList.ForEach(el =>
        {
            el.GetComponent<SkillDescription>().skillBorder.DOFade(0, 0);
        });
        selectedSkill.skillBorder.DOFade(1, 0);
    }
}
