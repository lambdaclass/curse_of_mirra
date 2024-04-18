using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VFXGraphHolder", menuName = "ScriptableObject/VFXGraphHolder")]
public class VFXGraphHolder : ScriptableObject
{
    [SerializeField] public GameObject[] vfx_graphs = null;
}
