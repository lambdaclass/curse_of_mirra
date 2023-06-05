using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SineUIControllerTopDownEffects : MonoBehaviour
{

    public CanvasGroup canvasGroup;
    public PrefabSpawner prefabSpawnerObject;
    public Text nameInUI;

    private string nameOfThePrafab;

    private void Start()
    {
        //
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            canvasGroup.alpha = 1f - canvasGroup.alpha;
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeEffect(true);
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeEffect(false);
        }
        if (Input.GetMouseButtonDown(1))
        {
            prefabSpawnerObject.SpawnPrefab();
        }

        nameOfThePrafab = prefabSpawnerObject.nameOfThePrefab;
        nameInUI.text = "Spawn - " + nameOfThePrafab;
    }

    // Change active VFX
    public void ChangeEffect(bool bo)
    {
        prefabSpawnerObject.ChangePrefabIntex(bo);
        nameOfThePrafab = prefabSpawnerObject.nameOfThePrefab;
    }
}
