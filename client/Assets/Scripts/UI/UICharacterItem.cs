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
        artWork.sprite = comCharacter.artWork;
        if (!IsActive())
        {
            Color newColor = new Color(0.3f, 0.4f, 0.6f);
            float grayscale = newColor.grayscale;
            artWork.color = new Color(grayscale, grayscale, grayscale);
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
                skillContainer.list[(int)UIControls.SkillBasic].SetSkillDescription(
                    comCharacter.skillsInfo[0]
                );
                skillContainer.list[(int)UIControls.Skill1].SetSkillDescription(
                    comCharacter.skillsInfo[1]
                );
                skillContainer.list[(int)UIControls.Skill2].SetSkillDescription(
                    comCharacter.skillsInfo[2]
                );
                skillContainer.list[(int)UIControls.Skill3].SetSkillDescription(
                    comCharacter.skillsInfo[3]
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
