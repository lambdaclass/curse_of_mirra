using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillDescription : MonoBehaviour, IPointerDownHandler
{
    SkillInfo skillData;
    public Sprite skillSprite;
    SkillsDetailHandler skillsDetailHandler;

    public void SetSkillDescription(SkillInfo skillInfo)
    {
        skillData = skillInfo;
        skillSprite = skillInfo.skillSprite;

        GetComponent<Image>().sprite = skillSprite;

        // The first list element always starts with a selected display
        GameObject firstGameObject = transform.parent.GetComponent<SkillsDetailHandler>().list[
            0
        ].gameObject;
        if (this.gameObject == firstGameObject)
        {
            GetComponent<Image>().color = new Color(255, 255, 255);
            skillsDetailHandler = transform.parent.GetComponent<SkillsDetailHandler>();
            skillsDetailHandler.SetSkillDetaill(skillData.name, skillData.description);
        }
        else
        {
            GetComponent<Image>().color = new Color(0.1f, 0.1f, 0.1f);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        skillsDetailHandler = transform.parent.GetComponent<SkillsDetailHandler>();
        skillsDetailHandler.SetSkillDetaill(skillData.name, skillData.description);
        skillsDetailHandler.ResetSelectSkill(skillSprite, new Color(0.1f, 0.1f, 0.1f));
        GetComponent<Image>().color = new Color(255f, 255f, 255f);
    }
}
