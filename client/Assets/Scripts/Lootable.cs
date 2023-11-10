using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Lootable", menuName = "CoM Lootable")]
public class Lootable : ScriptableObject
{
    public string lootName;
    public GameObject lootPrefab;
    public AudioClip pickUpSound;
    public string type;
}
