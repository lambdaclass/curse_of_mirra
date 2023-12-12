using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharactersListManager : MonoBehaviour
{
    [SerializeField]
    List<CoMCharacter> characterSriptableObjects;

    [SerializeField]
    GameObject listItem;

    [SerializeField]
    Sprite unavailableSprite;

    [SerializeField]
    Sprite unavailableIcon;

    void Start()
    {
        GenerateList();
    }

    void GenerateList()
    {
        var index = 0;
        var avaibles = Utils.GetOnlyAvailableCharacterInfo(characterSriptableObjects);
        characterSriptableObjects.ForEach(
            (character) =>
            {
                GameObject item = Instantiate(listItem, this.transform);
                CharacterListItem instanceListItem = item.GetComponent<CharacterListItem>();
                // Characters enabled to select
                if (avaibles.Contains(character))
                {
                    instanceListItem.listPosition = index;
                    index++;

                    instanceListItem.soonLabel.SetActive(false);
                    instanceListItem.characterOpacity.SetActive(false);
                    instanceListItem.characterIconState.sprite = character.characterIcon;
                }
                // Characters unable to select
                else
                {
                    instanceListItem.listPosition = -1;
                    instanceListItem.IsEnable = false;
                    item.GetComponent<ButtonAnimationsMMTouchButton>().enabled = false;

                    instanceListItem.characterIconState.sprite = unavailableIcon;
                }
                // Character's that we can't see preview of
                if (character.characterSprite == null)
                {
                    instanceListItem.characterImage.sprite = unavailableSprite;
                    instanceListItem.availableSoon.SetActive(true);
                }
                // Character's with preview but we can't select
                else
                {
                    instanceListItem.characterImage.sprite = character.characterSprite;
                    instanceListItem.characterName.gameObject.SetActive(true);
                    instanceListItem.characterName.text = character.name;
                }
            }
        );
    }
}
