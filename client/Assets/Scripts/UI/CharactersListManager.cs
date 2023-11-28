using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class CharactersListManager : MonoBehaviour
{
    [SerializeField]
    List<CoMCharacter> characterSriptableObjects;

    [SerializeField]
    GameObject listItem;

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
                //We only activates the charactrs which are in the avaibles list
                if (avaibles.Contains(character))
                {
                    item.GetComponent<CharacterListItem>().listPosition = index;
                    index++;
                }
                else
                {
                    item.GetComponent<CharacterListItem>().listPosition = -1;
                    item.GetComponent<CharacterListItem>().IsEnable = false;
                    item.GetComponent<ButtonAnimationsMMTouchButton>().enabled = false;
                }
                item.GetComponentInChildren<Image>().sprite = character.characterSprite;
                item.GetComponentInChildren<TextMeshProUGUI>().text = character.name;
            }
        );
    }
}
