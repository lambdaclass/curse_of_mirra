using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillDescription : MonoBehaviour, IPointerDownHandler
{
    SkillInfo skillData;
    public Sprite skillSprite;
    public Sprite selectedSkillSprite;

    public void SetSkillDescription(SkillInfo skillInfo, Sprite skill, Sprite selectedSkill)
    {
        skillData = skillInfo;
        skillSprite = skill;
        selectedSkillSprite = selectedSkill;

        // The first list element always starts with a selected display
        if (
            this.gameObject.name
            == transform.parent.GetComponent<SkillsDetailHandler>().GetAllChilds()[0].name
        )
        {
            GetComponent<Image>().sprite = selectedSkillSprite;
        }
        else
        {
            GetComponent<Image>().sprite = skillSprite;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.parent
            .GetComponent<SkillsDetailHandler>()
            .SetSkillDetaill(skillData.name, skillData.description);
        transform.parent
            .GetComponent<SkillsDetailHandler>()
            .SetSkillIcon(skillSprite, selectedSkillSprite);
    }
}
