using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharactersList : MonoBehaviour
{
    public static CharactersList Instance;

    [SerializeField]
    private List<CoMCharacter> allCharacters;

    public List<CoMCharacter> AllCharacters
    {
        get { return allCharacters; }
        private set { allCharacters = value; }
    }

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
        allCharacters = allCharacters.OrderByDescending(character => character.enabled).ToList();
        availableCharacters = allCharacters.Where(character => character.enabled).ToList();
    }
}
