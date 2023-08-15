using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Looteable", menuName = "Looteable")]
public class Looteable : ScriptableObject
{
    public string lootName;
    public GameObject prefab;
    public GameObject pickUpEffect;
}
