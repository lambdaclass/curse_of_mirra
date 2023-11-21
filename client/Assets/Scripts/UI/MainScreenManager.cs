using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScreenManager : MonoBehaviour
{
    [SerializeField]
    UIModelManager modelManager;

    void Start()
    {
        modelManager.SetModel();
    }
}
