using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CoM Character", menuName = "CoM Character")]
public class CoMCharacter : ScriptableObject
{
    public new string name;
    public Sprite artWork;
    public Sprite selectedArtwork;
    public bool selected = false;
    public GameObject prefab;
}
