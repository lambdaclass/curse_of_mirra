using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersList : MonoBehaviour
{
    public static CharactersList Instance;

    [SerializeField]
    List<CoMCharacter> availableCharacters;

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
