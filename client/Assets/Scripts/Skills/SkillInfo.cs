using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

[CreateAssetMenu(fileName = "New Skill Info", menuName = "CoM Skill")]
public class SkillInfo : ScriptableObject
{
    public new string name;
    public string description;
    public UIType inputType;
    public UIIndicatorType indicatorType;
    public GameObject feedbackVfx;
    public float feedbackVfxDuration;
    public GameObject startFeedbackVfx;
    public float startFeedbackVfxDuration;
    public bool instantiateVfxOnModel;
    public float animationSpeedMultiplier;
    public bool hasModelAnimation;
    public AudioClip abilityStartSfx;
    public float startAnimationDuration;
    public float executeAnimationDuration;
    public float skillCircleRadius;

    [MMEnumCondition("indicatorType", (int)UIIndicatorType.Cone)]
    public float skillConeAngle;

    [MMEnumCondition("indicatorType", (int)UIIndicatorType.Arrow)]
    public float arrowWidth;

    [MMEnumCondition("indicatorType", (int)UIIndicatorType.Area)]
    public float skillAreaRadius;
}
