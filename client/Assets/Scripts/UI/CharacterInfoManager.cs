using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MoreMountains.Tools;
using System.Linq;
using System;
using System.Text;
using UnityEngine.Networking;

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

    public static int selectedCharacterPosition;

    private List<CoMCharacter> availableCharacters = new List<CoMCharacter>();

    void Start()
    {
        availableCharacters = CharactersList.Instance.AvailableCharacters;
        SetCharacterInfo(selectedCharacterPosition);
    }

    public void RightArrowFunc()
    {
        if (
            selectedCharacterPosition
            == availableCharacters.Count(character => character.enabled) - 1
        )
        {
            selectedCharacterPosition = 0;
        }
        else
        {
            selectedCharacterPosition = selectedCharacterPosition + 1;
        }

        SetCharacterInfo(selectedCharacterPosition);
    }

    public void LeftArrowFunc()
    {
        if (selectedCharacterPosition == 0)
        {
            selectedCharacterPosition =
                availableCharacters.Count(character => character.enabled) - 1;
        }
        else
        {
            selectedCharacterPosition = selectedCharacterPosition - 1;
        }
        SetCharacterInfo(selectedCharacterPosition);
    }

    public void SetCharacterInfo(int currentPosition)
    {
        CoMCharacter comCharacter = availableCharacters[currentPosition];
        ModelManager.RemoveCurrentModel();
        ModelManager.SetModel(comCharacter);
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
            Utils.SetSelectedCharacter(
                availableCharacters[selectedCharacterPosition].name,
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
