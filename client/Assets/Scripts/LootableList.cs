using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CoM Lootable List", fileName = "Lootable List")]
public class LootableList : ScriptableObject
{
    public List<Lootable> LootList;
}
