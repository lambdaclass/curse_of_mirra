using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixMode : MonoBehaviour
{
    [SerializeField] bool isActive;
    GameObject grid;

    void Start(){
        grid = GameObject.Find("Grid");
    }

    public void ToggleMatrixMode(){
        isActive = !isActive;

        if (grid)
        {
            grid.transform.GetChild(0).gameObject.SetActive(isActive);
        }

        GameObject[] hitboxes = GameObject.FindGameObjectsWithTag("Hitbox");
        foreach (GameObject hitbox in hitboxes)
        {
            hitbox.transform.GetChild(0).gameObject.SetActive(isActive);
        }
    }
}
