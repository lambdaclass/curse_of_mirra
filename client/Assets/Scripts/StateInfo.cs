using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New State", menuName = "CoM State")]
public class StateInfo : ScriptableObject
{
    public uint id;
    public string stateName;
    public Sprite image;
    public float duration;
}
