using System.Collections.Generic;
using UnityEngine;

public class CharactersListManager : MonoBehaviour
{
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
        CharactersManager.Instance.AllCharacters.ForEach(
            (character) =>
            {
                GameObject item = Instantiate(listItem, this.transform);
                CharacterListItem instanceListItem = item.GetComponent<CharacterListItem>();
                // Assign the character's name to be link to character info
                instanceListItem.characterNameString = character.name;

                // Characters enabled to select
                if (CharactersManager.Instance.IsAvaibleCharacter(character))
                {
                    instanceListItem.soonLabel.SetActive(false);
                    instanceListItem.characterOpacity.SetActive(false);
                    instanceListItem.characterIconState.sprite = character.characterIcon;
                }
                // Characters unable to select
                else
                {
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
