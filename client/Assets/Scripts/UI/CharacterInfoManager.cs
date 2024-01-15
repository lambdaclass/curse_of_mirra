using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    private int characterIndex;

    List<CoMCharacter> availableCharacters;

    void Start()
    {
        availableCharacters = CharactersManager.Instance.AvailableCharacters;
        string goToCharacter = CharactersManager.Instance.GetGoToCharacter();

        // Get index from selected character to show previous and next character
        characterIndex = availableCharacters.FindIndex(
            availableCharacter => availableCharacter.name == goToCharacter
        );

        CoMCharacter characterToShow = availableCharacters.Find(
            character => character.name == goToCharacter
        );

        rightButton.enabled = (availableCharacters.Count() > 1);
        leftButton.enabled = (availableCharacters.Count() > 1);
        SetCharacterInfo(characterToShow);
    }

    public void RightArrowFunction()
    {
        if (availableCharacters.Count() > 1)
        {
            if (characterIndex == availableCharacters.Count() - 1)
            {
                characterIndex = 0;
            }
            else
            {
                characterIndex += 1;
            }
            SetCharacterInfo(availableCharacters[characterIndex]);
        }
    }

    public void LeftArrowFunction()
    {
        if (availableCharacters.Count() > 1)
        {
            if (characterIndex == 0)
            {
                characterIndex = availableCharacters.Count() - 1;
            }
            else
            {
                characterIndex -= 1;
            }
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
        CharactersManager.Instance.SetGoToCharacter(comCharacter.name);
    }

    public void SelectButton()
    {
        StartCoroutine(SetCharacter());
    }

    private IEnumerator SetCharacter()
    {
        yield return StartCoroutine(
            ServerUtils.SetSelectedCharacter(
                CharactersManager.Instance.GetGoToCharacter(),
                response =>
                {
                    ServerConnection.Instance.selectedCharacterName = response.selected_character;
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
