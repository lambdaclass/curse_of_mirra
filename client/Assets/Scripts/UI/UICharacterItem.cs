using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICharacterItem : MonoBehaviour, IPointerDownHandler
{
    public CoMCharacter comCharacter;
    public new TextMeshProUGUI name;
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI skillDescription;

    public Image artWork;
    public bool selected = false;

    [SerializeField]
    public GameObject characterDescription;

    [SerializeField]
    public SkillsDetailHandler skillContainer;

    [SerializeField]
    public ConfirmButtonHandler confirmButton;

    void Start()
    {
        if (IsActive())
        {
            artWork.sprite = comCharacter.artWork;
        }
        else
        {
            artWork.sprite = comCharacter.blockArtwork;
        }
    }

    public bool IsActive()
    {
        // var charactersList = ServerConnection.Instance.serverSettings.CharacterConfig.Items;
        // foreach (var character in charactersList)
        // {
        //     if (comCharacter.name == character.Name)
        //     {
        //         return int.Parse(character.Active) == 1;
        //     }
        // }
        return false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameServerConnectionManager.Instance.isConnectionOpen())
        {
            if (IsActive())
            {
                characterDescription.SetActive(true);
                selected = true;
                artWork.sprite = comCharacter.selectedArtwork;
                name.text = comCharacter.name;
                skillName.text = comCharacter.skillBasicInfo.name;
                skillDescription.text = comCharacter.skillBasicInfo.description;
                skillContainer
                    .skillsList[(int)UIControls.SkillBasic]
                    .SetSkillDescription(comCharacter.skillsInfo[0]);
                skillContainer
                    .skillsList[(int)UIControls.Skill1]
                    .SetSkillDescription(comCharacter.skillsInfo[1]);
                transform
                    .parent
                    .GetComponent<CharacterSelectionUI>()
                    .DeselectCharacters(comCharacter.name);
                transform.parent.GetComponent<CharacterSelectionUI>().selectedCharacterName =
                    comCharacter.name;

                confirmButton.HandleButton();
            }
        }
    }
}
