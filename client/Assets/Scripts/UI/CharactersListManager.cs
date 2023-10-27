using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharactersListManager : MonoBehaviour
{
    [SerializeField]
    List<CoMCharacter> players;

    [SerializeField]
    GameObject listItem;

    // Start is called before the first frame update
    void Start()
    {
        GenerateList();
    }

    void GenerateList()
    {
        players.ForEach(player =>
        {
            GameObject item = (GameObject)Instantiate(listItem, this.transform);
            item.GetComponentInChildren<Image>().sprite = player.characterSprite;
            item.GetComponentInChildren<TextMeshProUGUI>().text = player.name;
        });
    }
}
