using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class CharacterInfoManager : MonoBehaviour
{
    [SerializeField]
    private UIModelManager modelManager;

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

    [Header("Arrows")]
    [SerializeField]
    ButtonAnimationsMMTouchButton leftButton,
        rightButton;

    public static int characterIndex;

    List<CoMCharacter> availableCharacters;

    void Start()
    {
        availableCharacters = CharactersManager.Instance.availableCharacters;

        CoMCharacter selectedCharacter = availableCharacters.Single(
            comCharacter => comCharacter.name == CharactersManager.Instance.selectedCharacterName
        );
        characterIndex = availableCharacters.FindIndex(
            availableCharacter => availableCharacter.name == selectedCharacter.name
        );

        if (availableCharacters.Count() > 1)
        {
            rightButton.enabled = true;
            leftButton.enabled = true;
        }
        else
        {
            rightButton.enabled = false;
            leftButton.enabled = false;
        }
        SetCharacterInfo(selectedCharacter);
    }

    public void RightArrowFunction()
    {
        if (characterIndex == availableCharacters.Count - 1)
        {
            characterIndex = 0;
        }
        else
        {
            characterIndex = characterIndex + 1;
        }
        if (availableCharacters.Count() > 1)
        {
            SetCharacterInfo(availableCharacters[characterIndex]);
        }
    }

    public void LeftArrowFunction()
    {
        if (characterIndex == 0)
        {
            characterIndex = availableCharacters.Count - 1;
        }
        else
        {
            characterIndex = characterIndex - 1;
        }
        if (availableCharacters.Count() > 1)
        {
            SetCharacterInfo(availableCharacters[characterIndex]);
        }
    }

    public void SetCharacterInfo(CoMCharacter comCharacter)
    {
        modelManager.RemoveCurrentModel();
        modelManager.SetModel(comCharacter.name);
        nameText.text = comCharacter.name;
        subTitle.text = comCharacter.description;
        classImage.sprite = comCharacter.classImage;
        skillDescriptions[0].SetSkillDescription(comCharacter.skillsInfo[0]);
        skillDescriptions[1].SetSkillDescription(comCharacter.skillsInfo[1]);
        StartCoroutine(modelManager.GetComponentInChildren<RotateUIModel>().GetModel());
    }
}
