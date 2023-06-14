using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using TMPro;
using UnityEngine;

public class DisplayPlayerName : MonoBehaviour
{
    [SerializeField] Character character;
    void Start()
    {
        GetComponent<TextMeshPro>().text = "Player " + character.PlayerID;
    }

    void Update()
    {
        if (character.GetComponent<Health>().CurrentHealth <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }
}
