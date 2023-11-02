using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

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

    [Header("Buttons")]
    [SerializeField]
    GameObject leftButton;

    [SerializeField]
    GameObject rightButton;

    int currentCharacterListPosition = 0;
    public static int selectedCharacterPosition;

    void Start()
    {
        SetCharacterInfo(selectedCharacterPosition);
    }

    public void RightArrowFunc()
    {
        if (currentCharacterListPosition == comCharacters.Count - 1)
        {
            currentCharacterListPosition = 0;
        }
        else
        {
            currentCharacterListPosition = currentCharacterListPosition + 1;
        }

        SetCharacterInfo(currentCharacterListPosition);
    }

    public void LeftArrowFunc()
    {
        if (currentCharacterListPosition == 0)
        {
            currentCharacterListPosition = comCharacters.Count - 1;
        }
        else
        {
            currentCharacterListPosition = currentCharacterListPosition - 1;
        }
        SetCharacterInfo(currentCharacterListPosition);
    }

    public void SetCharacterInfo(int currentCharacterListPosition)
    {
        CoMCharacter comCharacter = comCharacters[currentCharacterListPosition];
        ModelManager.RemoveCurrentModel();
        ModelManager.SetModel(comCharacter);
        nameText.text = comCharacter.name;
        subTitle.text = comCharacter.description;
        classImage.sprite = comCharacter.classImage;
        skillDescriptions[0].SetSkillDescription(comCharacter.skillsInfo[0]);
        skillDescriptions[1].SetSkillDescription(comCharacter.skillsInfo[1]);
    }
}
