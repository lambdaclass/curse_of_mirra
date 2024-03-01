using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MoreMountains.Tools;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill Info", menuName = "CoM Skill")]
public class SkillInfo : ScriptableObject
{
    [NonSerialized] public ulong ownerId;
    public new string name;
    public string description;
    public UIType inputType;

    public UIControls skillSetType;
    public float angle;
    public ulong staminaCost;
    public UIIndicatorType indicatorType;

    public bool hasProjectile;

    [MMCondition("hasProjectile", true)]
    public GameObject projectilePrefab;

    [MMCondition("hasProjectile", true)]
    public string skillKey;
    public float animationSpeedMultiplier;
    public AudioClip abilityStartSfx;

    public bool sfxHasAbilityStop;

    [MMCondition("sfxHasAbilityStop", true)]
    public AudioClip abilityStopSfx;
    public float skillCircleRadius;

    [MMEnumCondition("indicatorType", (int)UIIndicatorType.Cone)]
    public float skillConeAngle;

    [MMEnumCondition("indicatorType", (int)UIIndicatorType.Arrow)]
    public float arrowWidth;

    [MMEnumCondition("indicatorType", (int)UIIndicatorType.Area)]
    public float skillAreaRadius;
    public bool showCooldown;
    public float damage;
    public float cooldown;
    public float skillRange;
    public Sprite skillSprite;

    [Header("Feedbacks")]
    [SerializeField]
    public List<VfxStep> vfxList;
    public List<AnimationStep> animationList;

    // public bool Equals(SkillConfigItem skillConfigItem)
    // {
    //     return this.name.ToLower() == skillConfigItem.Name.ToLower();
    // }

    public void InitWithBackend(ConfigSkill configSkill, string id)
    {
        var range = configSkill.TargettingAngle != 0 ? (configSkill.TargettingRadius / 100) / 2 : configSkill.TargettingRadius / 100;
        Debug.Log(configSkill.Name);
        Debug.Log(range);
        // Issue #1419
        this.damage = 0;
        this.cooldown = configSkill.CooldownMs / 1000;
        this.skillCircleRadius = range;
        this.angle = configSkill.TargettingAngle;
        this.skillConeAngle = configSkill.TargettingAngle;
        this.ownerId = Convert.ToUInt64(id);
        this.staminaCost = configSkill.StaminaCost;
    }
}
