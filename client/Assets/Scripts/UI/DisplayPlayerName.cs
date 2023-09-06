using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using TMPro;
using UnityEngine;

public class DisplayPlayerName : MonoBehaviour
{
    [SerializeField]
    CustomCharacter character;

    void Start()
    {
        GetComponent<TextMeshPro>().text = "Player " + character.PlayerID;
    }

    void Update()
    {
        bool isAlive = character.GetComponent<Health>().CurrentHealth > 0;
        this.gameObject.SetActive(isAlive);
    }
}
