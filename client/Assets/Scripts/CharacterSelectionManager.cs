using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterSelectionManager : MonoBehaviour
{
    [SerializeField]
    CharacterSelectionList playersList;

    [SerializeField]
    CharacterSelectionUI characterList;
    public bool selected = false;

    void Start()
    {
        playersList.CreatePlayerItems();
    }

    void Update()
    {
        playersList.DisplayUpdates();
    }
}
