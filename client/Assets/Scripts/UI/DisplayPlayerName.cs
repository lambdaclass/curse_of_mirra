using MoreMountains.TopDownEngine;
using System;
using TMPro;
using UnityEngine;

public class DisplayPlayerName : MonoBehaviour
{
    [SerializeField]
    CustomCharacter character;

    void Start()
    {
        GetComponent<TextMeshPro>().text = LobbyConnection.Instance.username;
    }

    void Update()
    {
        bool isAlive = character.GetComponent<Health>().CurrentHealth > 0;
        this.gameObject.SetActive(isAlive);
    }

    public void SetName(string name)
    {
        GetComponent<TextMeshPro>().text = name;
    }
}
