using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterSelectionManager : MonoBehaviour
{
    [SerializeField]
    CharacterSelectionList playersList;

    void Start()
    {
        playersList.CreatePlayerItems();
    }

    void Update()
    {
        playersList.DisplayUpdates();
    }
}
