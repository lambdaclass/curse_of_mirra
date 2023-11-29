using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class CharactersListManager : MonoBehaviour
{
    [SerializeField]
    GameObject listItem;

    void Start()
    {
        GenerateList();
    }

    void GenerateList()
    {
        var index = 0;
        CharactersList.Instance.AvailableCharacters.ForEach(
            (character) =>
            {
                GameObject item = Instantiate(listItem, this.transform);
                if (character.enabled)
                {
                    item.GetComponent<CharacterListItem>().listPosition = index;
                    item.GetComponentInChildren<Image>().sprite = character.characterSprite;
                    index++;
                }
                else
                {
                    item.GetComponent<CharacterListItem>().listPosition = -1;
                    item.GetComponent<CharacterListItem>().IsEnable = false;
                    item.GetComponent<ButtonAnimationsMMTouchButton>().enabled = false;
                    item.GetComponentInChildren<Image>().sprite = character.disabledCharacterSprite;
                }
                item.GetComponentInChildren<TextMeshProUGUI>().text = character.name;
            }
        );
    }
}
