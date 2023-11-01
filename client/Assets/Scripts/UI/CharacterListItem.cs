using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterListItem : MonoBehaviour
{
    public int listPosition;

    public void SetCharacterInfoStart()
    {
        CharacterInfoManager.startPos = listPosition;
    }
}
