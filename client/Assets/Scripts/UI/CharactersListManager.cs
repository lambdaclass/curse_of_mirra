using System.Collections;
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

    void Start()
    {
        GenerateList();
    }

    void GenerateList()
    {
        var index = 0;
        characterSriptableObjects.ForEach(
            (character) =>
            {
                GameObject item = Instantiate(listItem, this.transform);
                item.GetComponent<CharacterListItem>().listPosition = index;
                item.GetComponentInChildren<Image>().sprite = character.characterSprite;
                item.GetComponentInChildren<TextMeshProUGUI>().text = character.name;
                index++;
            }
        );
    }
}
