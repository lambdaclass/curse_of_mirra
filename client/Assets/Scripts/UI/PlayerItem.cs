using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviour
{
    [SerializeField] public Text playerText;
    [SerializeField] public Text playerCharacterText;
    public int id;

    public void SetCharacterText(string name){
        this.playerCharacterText.text = name;
    }

    public int GetId(){
        return id;
    }
}
