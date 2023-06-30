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
    public SkillInfo skillBasicInfo;
    public SkillInfo skill1Info;
    public SkillInfo skill2Info;
    public SkillInfo skill3Info;
    public SkillInfo skill4Info;
}
