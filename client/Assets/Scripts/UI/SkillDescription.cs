using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillDescription : MonoBehaviour, IPointerDownHandler
{
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI skillDescription;
    SkillInfo skillData;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("here");
    }

    public void GetCharacter(SkillInfo skill)
    {
        Debug.Log("get: " + skill.name);
        skillData = skill;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("pointer down: " + this.name);
        Debug.Log("skill: " + skillData);
        skillName.text = skillData.name;
        skillDescription.text = skillData.description;
    }
}
