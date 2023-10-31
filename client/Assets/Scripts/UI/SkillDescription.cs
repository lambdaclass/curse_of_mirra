using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillDescription : MonoBehaviour, IPointerDownHandler
{
    SkillInfo skillData;
    Sprite skillSprite;

    [SerializeField]
    public Image skillBorder;

    [SerializeField]
    SkillsDetailHandler skillsDetailHandler;

    public void SetSkillDescription(SkillInfo skillInfo)
    {
        skillData = skillInfo;
        skillSprite = skillInfo.skillSprite;

        GetComponent<Image>().sprite = skillSprite;

        // The first list element always starts with a selected display
        GameObject firstGameObject = skillsDetailHandler.skillsList[0].gameObject;
        if (this.gameObject == firstGameObject)
        {
            skillsDetailHandler.ResetSelectSkill(this);
            skillsDetailHandler.SetSkillDetaill(skillData.name, skillData.description);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        skillsDetailHandler.SetSkillDetaill(skillData.name, skillData.description);
        skillsDetailHandler.ResetSelectSkill(this);
    }
}
