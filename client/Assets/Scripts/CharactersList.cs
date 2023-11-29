using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharactersList : MonoBehaviour
{
    public static CharactersList Instance;

    [SerializeField]
    private List<CoMCharacter> availableCharacters;

    public List<CoMCharacter> AvailableCharacters
    {
        get { return availableCharacters; }
        private set { availableCharacters = value; }
    }

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        availableCharacters = availableCharacters.OrderByDescending(character => character.enabled).ToList();
    }
}
