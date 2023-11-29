using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainScreenManager : MonoBehaviour
{
    [SerializeField]
    UIModelManager modelManager;

    // We could map this with the backend
    // Add Uma to the list of string to visualize her
    [System.NonSerialized]
    public static List<string> enabledCharactersName = new List<string> { "Muflus", "Uma" };

    void Awake()
    {
        modelManager.SetupList(enabledCharactersName);
    }

    void Start()
    {
        modelManager.SetModel();
    }
}
