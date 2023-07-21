using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill Info", menuName = "CoM Skill")]
public class SkillInfo : ScriptableObject
{
    public new string name;
    public UIType inputType;
    public UIIndicatorType indicatorType;
    public GameObject feedbackAnimation;
    public bool instantiateAnimationOnModel;
    public float animationSpeedMultiplier;
    public bool hasModelAnimation;
    public AudioClip abilityStartSfx;
    public float skillCircleRadius;
    public float skillConeAngle;
}
