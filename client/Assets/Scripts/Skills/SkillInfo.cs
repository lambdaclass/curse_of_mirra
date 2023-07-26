using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill Info", menuName = "CoM Skill")]
public class SkillInfo : ScriptableObject
{
    public new string name;
    public string description;
    public UIType inputType;
    public GameObject feedbackVfx;
    public float feedbackVfxDuration;
    public GameObject startFeedbackVfx;
    public float startFeedbackVfxDuration;
    public bool instantiateVfxOnModel;
    public float animationSpeedMultiplier;
    public bool hasModelAnimation;
    public AudioClip abilityStartSfx;
}
