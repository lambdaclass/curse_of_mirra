using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharactersListManager : MonoBehaviour
{
    [SerializeField]
    GameObject listItem;

    // Start is called before the first frame update
    void Start()
    {
        GenerateList();
    }

    void GenerateList()
    {
        List<GameObject> players = SocketConnectionManager.Instance.players;
        players.ForEach(el =>
        {
            GameObject item = (GameObject)Instantiate(listItem, this.transform);
            //item.GetComponent<Image>().sprite = CoMCharacter.characterSprite;
        });
    }
}
