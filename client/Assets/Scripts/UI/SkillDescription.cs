using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillDescription : MonoBehaviour, IPointerDownHandler
{
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI skillDescription;
    SkillInfo skillData;

    // Start is called before the first frame update
    void Start() { }

    public void GetCharacter(SkillInfo skill)
    {
        skillData = skill;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        skillName.text = skillData.name;
        skillDescription.text = skillData.description;
    }
}
