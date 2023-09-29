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
    public Sprite skillBackground;
    public Color32 InputFeedbackColor;
    public List<SkillInfo> skillsInfo;

    // The references below should be deleted with an upcoming refactor
    public SkillInfo skillBasicInfo;
    public Sprite skillBasicSprite;
    public Sprite skill1Sprite;
    public Sprite skill2Sprite;
    public Sprite skill3Sprite;
    public Sprite skill4Sprite;
    public List<Sprite> selectedSkills;
    public List<Sprite> notSelectedSkills;
}
