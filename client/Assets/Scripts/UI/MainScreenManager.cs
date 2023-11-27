using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainScreenManager : MonoBehaviour
{
    [SerializeField]
    UIModelManager modelManager;

    // We could map this with the backend
    [System.NonSerialized]
    public static List<string> enableCharactersName = new List<string> { "Muflus" };

    void Awake()
    {
        modelManager.SetupList(enableCharactersName);
    }

    void Start()
    {
        modelManager.SetModel();
    }
}
