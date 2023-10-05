using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;

public class UICharacterItem : MonoBehaviour, IPointerDownHandler
{
    public CoMCharacter comCharacter;
    public TextMeshProUGUI name;
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI skillDescription;

    [SerializeField]
    public CharacterSelectionList PlayersList;

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
        var charactersList = LobbyConnection.Instance.serverSettings.CharacterConfig.Items;
        foreach (var character in charactersList)
        {
            if (comCharacter.name == character.Name)
            {
                return int.Parse(character.Active) == 1;
            }
        }
        return false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (SocketConnectionManager.Instance.isConnectionOpen())
        {
            if (IsActive())
            {
                characterDescription.SetActive(true);
                selected = true;
                artWork.sprite = comCharacter.selectedArtwork;
                name.text = comCharacter.name;
                skillName.text = comCharacter.skillBasicInfo.name;
                skillDescription.text = comCharacter.skillBasicInfo.description;
                skillContainer.skillsList[(int)UIControls.SkillBasic].SetSkillDescription(
                    comCharacter.skillsInfo[0]
                );
                skillContainer.skillsList[(int)UIControls.Skill1].SetSkillDescription(
                    comCharacter.skillsInfo[1]
                );
                skillContainer.skillsList[(int)UIControls.Skill2].SetSkillDescription(
                    comCharacter.skillsInfo[2]
                );
                skillContainer.skillsList[(int)UIControls.Skill3].SetSkillDescription(
                    comCharacter.skillsInfo[3]
                );
                skillContainer.skillsList[(int)UIControls.Skill4].SetSkillDescription(
                    comCharacter.skillsInfo[4]
                );
                transform.parent
                    .GetComponent<CharacterSelectionUI>()
                    .DeselectCharacters(comCharacter.name);
                transform.parent.GetComponent<CharacterSelectionUI>().selectedCharacterName =
                    comCharacter.name;

                confirmButton.HandleButton();
            }
        }
    }
}
