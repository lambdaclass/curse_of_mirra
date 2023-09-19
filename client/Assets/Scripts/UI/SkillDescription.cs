using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillDescription : MonoBehaviour, IPointerDownHandler
{
    SkillInfo skillData;
    public Sprite skillSprite;
    public Sprite selectedSkillSprite;
    SkillsDetailHandler skillsDetailHandler;

    public void SetSkillDescription(SkillInfo skillInfo)
    {
        skillData = skillInfo;
        skillSprite = skillInfo.skillSprites[0];
        selectedSkillSprite = skillInfo.skillSprites[1];

        // The first list element always starts with a selected display
        GameObject firstGameObject = transform.parent.GetComponent<SkillsDetailHandler>().list[
            0
        ].gameObject;
        if (this.gameObject == firstGameObject)
        {
            GetComponent<Image>().sprite = selectedSkillSprite;
            skillsDetailHandler = transform.parent.GetComponent<SkillsDetailHandler>();
            skillsDetailHandler.SetSkillDetaill(skillData.name, skillData.description);
        }
        else
        {
            GetComponent<Image>().sprite = skillSprite;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        skillsDetailHandler = transform.parent.GetComponent<SkillsDetailHandler>();
        skillsDetailHandler.SetSkillDetaill(skillData.name, skillData.description);
        skillsDetailHandler.SetSkillIcon(skillSprite, selectedSkillSprite);
    }
}
