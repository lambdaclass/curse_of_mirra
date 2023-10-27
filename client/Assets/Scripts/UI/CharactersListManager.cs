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

    // Start is called before the first frame update
    void Start()
    {
        GenerateList();
    }

    void GenerateList()
    {
        characterSriptableObjects.ForEach(character =>
        {
            GameObject item = (GameObject)Instantiate(listItem, this.transform);
            item.GetComponentInChildren<Image>().sprite = character.characterSprite;
            item.GetComponentInChildren<TextMeshProUGUI>().text = character.name;
        });
    }
}
