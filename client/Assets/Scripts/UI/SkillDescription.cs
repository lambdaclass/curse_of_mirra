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
        GameObject firstGameObject = transform.parent.GetComponent<SkillsDetailHandler>().list[0];
        if (this.gameObject == firstGameObject)
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
        SkillsDetailHandler skillsDetailHandler =
            transform.parent.GetComponent<SkillsDetailHandler>();
        skillsDetailHandler.SetSkillDetaill(skillData.name, skillData.description);
        skillsDetailHandler.SetSkillIcon(skillSprite, selectedSkillSprite);
    }
}
