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
    public GameObject confirmButton;

    [SerializeField]
    public Image skillBasicSprite;

    [SerializeField]
    public Image skill1Sprite;

    [SerializeField]
    public Image skill2Sprite;

    [SerializeField]
    public Image skill3Sprite;

    [SerializeField]
    public Image skill4Sprite;

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
                confirmButton.SetActive(true);
                selected = true;
                artWork.sprite = comCharacter.selectedArtwork;
                name.text = comCharacter.name;
                skillName.text = comCharacter.skillBasicInfo.name;
                skillDescription.text = comCharacter.skillBasicInfo.description;
                skillBasicSprite.sprite = comCharacter.skillBasicSprite;
                skill1Sprite.sprite = comCharacter.skill1Sprite;
                skill2Sprite.sprite = comCharacter.skill2Sprite;
                skill3Sprite.sprite = comCharacter.skill3Sprite;
                skill4Sprite.sprite = comCharacter.skill4Sprite;

                skillBasicSprite
                    .GetComponent<SkillDescription>()
                    .GetCharacter(comCharacter.skillBasicInfo);
                skill1Sprite.GetComponent<SkillDescription>().GetCharacter(comCharacter.skill1Info);
                skill2Sprite.GetComponent<SkillDescription>().GetCharacter(comCharacter.skill2Info);
                skill3Sprite.GetComponent<SkillDescription>().GetCharacter(comCharacter.skill3Info);
                skill4Sprite.GetComponent<SkillDescription>().GetCharacter(comCharacter.skill4Info);

                transform.parent
                    .GetComponent<CharacterSelectionUI>()
                    .DeselectCharacters(comCharacter.name);

                transform.parent.GetComponent<CharacterSelectionUI>().selectedCharacterName =
                    comCharacter.name;
            }
        }
    }
}
