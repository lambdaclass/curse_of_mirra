using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MoreMountains.Tools;
using System.Linq;
using System;
using System.Text;
using UnityEngine.Networking;
using System.Collections;

public class CharacterInfoManager : MonoBehaviour
{
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

    [Header("Arrows")]
    [SerializeField]
    ButtonAnimationsMMTouchButton leftButton,
        rightButton;

    public static int characterIndex;

    List<CoMCharacter> availableCharacters;

    void Start()
    {
        availableCharacters = CharactersManager.Instance.AvailableCharacters;
        // Get index from selected character to show previous and next character
        characterIndex = availableCharacters.FindIndex(
            availableCharacter =>
                availableCharacter.name == CharactersManager.Instance.GoToCharacter
        );
        CoMCharacter characterToShow = availableCharacters.Find(
            character => character.name == CharactersManager.Instance.GoToCharacter
        );

        rightButton.enabled = (availableCharacters.Count() > 1);
        leftButton.enabled = (availableCharacters.Count() > 1);
        SetCharacterInfo(characterToShow);
    }

    public void RightArrowFunction()
    {
        if (characterIndex == availableCharacters.Count(character => character.enabled) - 1)
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
            characterIndex = availableCharacters.Count(character => character.enabled) - 1;
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
        ModelManager.RemoveCurrentModel();
        ModelManager.SetModel(comCharacter.name);
        nameText.text = comCharacter.name;
        subTitle.text = comCharacter.description;
        classImage.sprite = comCharacter.classImage;
        skillDescriptions[0].SetSkillDescription(comCharacter.skillsInfo[0]);
        skillDescriptions[1].SetSkillDescription(comCharacter.skillsInfo[1]);
        StartCoroutine(ModelManager.GetComponentInChildren<RotateUIModel>().GetModel());
    }

    public void SelectButton()
    {
        StartCoroutine(SetCharacter());
    }

    private IEnumerator SetCharacter()
    {
        yield return StartCoroutine(
            ServerUtils.SetSelectedCharacter(
                CharactersManager.Instance.GoToCharacter,
                response =>
                {
                    LobbyConnection.Instance.selectedCharacterName = response.selected_character;
                },
                error =>
                {
                    Errors.Instance.HandleNetworkError("Error", error);
                }
            )
        );
        this.GetComponent<MMLoadScene>().LoadScene();
    }
}
