using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CoM Character", menuName = "CoM Character")]
public class CoMCharacter : ScriptableObject
{
    public new string name;
    public Sprite artWork;
    public Sprite selectedArtwork;
    public Sprite blockArtwork;
    public Sprite characterPlayer;
    public GameObject prefab;
    public SkillInfo skillBasicInfo;
    public SkillInfo skill1Info;
    public SkillInfo skill2Info;
    public SkillInfo skill3Info;
    public SkillInfo skill4Info;
    public Sprite skillBasicSprite;
    public Sprite skill1Sprite;
    public Sprite skill2Sprite;
    public Sprite skill3Sprite;
    public Sprite skill4Sprite;
    public Sprite skillBackground;
    public List<Sprite> selectedSkills;
    public List<Sprite> notSelectedSkills;
    public List<SkillInfo> skillsInfo;
    public Color32 InputFeedbackColor;
}
