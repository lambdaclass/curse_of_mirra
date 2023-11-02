using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

public class CharacterListItem : MonoBehaviour
{
    public int listPosition;

    public void SetCharacterInfoStart()
    {
        this.GetComponent<MMLoadScene>().LoadScene();
        CharacterInfoManager.selectedCharacterPosition = listPosition;
    }
}
