using System.Collections.Generic;
using UnityEngine;

public class CharactersManager : MonoBehaviour
{
    public static CharactersManager Instance;

    [SerializeField]
    public List<CoMCharacter> characterSriptableObjects;
    public List<CoMCharacter> availableCharacters = new List<CoMCharacter>();

    // The available names should come from backend
    private List<string> availableCharacterNames = new List<string>() { "Muflus" };
    public string selectedCharacterName = "Muflus";

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        availableCharacters = Utils.GetOnlyAvailableCharacterInfo(availableCharacterNames);
    }
}
