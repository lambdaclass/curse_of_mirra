using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterInfoManager : MonoBehaviour
{
    [SerializeField]
    List<CoMCharacter> comCharacters;

    [SerializeField]
    private UIModelManager ModelManager;

    [Header("Character info")]
    [SerializeField]
    TextMeshProUGUI nameText;

    [SerializeField]
    Image classImage;

    [SerializeField]
    TextMeshProUGUI subTitle;

    [Header("Character skills")]
    [SerializeField]
    List<SkillDescription> skillDescriptions;

    [SerializeField]
    TextMeshProUGUI skillName;

    [SerializeField]
    TextMeshProUGUI skillDescription;

    [Header("Buttons")]
    [SerializeField]
    GameObject leftButton;

    [SerializeField]
    GameObject rightButton;

    int currentPos = 0;

    public static int startPos;

    void Start()
    {
        print(startPos);
        SetCharacterInfo(startPos);
    }

    public void RightArrowFunc()
    {
        if (currentPos == comCharacters.Count - 1)
        {
            currentPos = 0;
        }
        else
        {
            currentPos = currentPos + 1;
        }

        SetCharacterInfo(currentPos);
    }

    public void LeftArrowFunc()
    {
        if (currentPos == 0)
        {
            currentPos = comCharacters.Count - 1;
        }
        else
        {
            currentPos = currentPos - 1;
        }
        SetCharacterInfo(currentPos);
    }

    public void SetCharacterInfo(int currentPos)
    {
        CoMCharacter comCharacter = comCharacters[currentPos];
        ModelManager.RemoveCurrentMode();
        ModelManager.SetModel(comCharacter);
        nameText.text = comCharacter.name;
        subTitle.text = comCharacter.description;
        classImage.sprite = comCharacter.classImage;
        skillDescriptions[0].SetSkillDescription(comCharacter.skillsInfo[0]);
        skillDescriptions[1].SetSkillDescription(comCharacter.skillsInfo[1]);
    }
}
