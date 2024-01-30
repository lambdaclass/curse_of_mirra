using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    private List<string> availableCharacterNames = new List<string>() { "Muflus", "H4ck" };

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
        allCharacters = allCharacters
            .OrderByDescending(character => availableCharacterNames.Contains(character.name))
            .ToList();
        availableCharacters = allCharacters
            .Where(character => availableCharacterNames.Contains(character.name))
            .ToList();
    }

    public void SetGoToCharacter(string goToCharacter)
    {
        this.GoToCharacter = goToCharacter;
    }

    public string GetGoToCharacter()
    {
        return this.GoToCharacter;
    }

    public List<string> GetAvailableCharactersNames()
    {
        return availableCharacterNames;
    }

    public bool IsAvailableCharacter(CoMCharacter character)
    {
        return this.AvailableCharacters.Contains(character);
    }
}
