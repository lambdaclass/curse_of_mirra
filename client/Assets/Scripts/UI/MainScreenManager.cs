using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
