using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CoM Crate List", fileName = "Crate List")]
public class CrateList : ScriptableObject
{
    public List<CrateInfo> crateList;
}
