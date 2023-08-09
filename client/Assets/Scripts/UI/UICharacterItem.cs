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
    public SkillsDetailHandler skillList;

    [SerializeField]
    public ConfirmButtonHandler confirmButton;

    void Start()
    {
        if (isActive())
        {
            artWork.sprite = comCharacter.artWork;
        }
        else
        {
            artWork.sprite = comCharacter.blockArtwork;
        }
    }

    public bool isActive()
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
            if (isActive())
            {
                characterDescription.SetActive(true);
                selected = true;
                artWork.sprite = comCharacter.selectedArtwork;
                name.text = comCharacter.name;
                skillName.text = comCharacter.skillBasicInfo.name;
                skillDescription.text = comCharacter.skillBasicInfo.description;
                skillList.list.ForEach(el =>
                {
                    var skill = skillList.list.IndexOf(el);
                    el.GetComponent<SkillDescription>()
                        .SetSkillDescription(
                            comCharacter.skillsInfo[skill],
                            comCharacter.notSelectedSkills[skill],
                            comCharacter.selectedSkills[skill]
                        );
                });
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
