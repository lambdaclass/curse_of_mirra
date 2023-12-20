using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharactersManager : MonoBehaviour
{
    public static CharactersManager Instance;

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

    // The available names should come from backend
    private List<string> availableCharacterNames = new List<string>() { "Muflus" };

    public string GoToCharacter;

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
        // availableCharacters = allCharacters.Where(character => character.enabled).ToList();
        availableCharacters = allCharacters
            .Where(character => availableCharacterNames.Contains(character.name))
            .ToList();
    }
}
