using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MoreMountains.Tools;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill Info", menuName = "CoM Skill")]
public class SkillInfo : ScriptableObject
{
    public new string name;
    public string description;
    public UIType inputType;

    public UIControls skillSetType;
    public float angle;

    [MMEnumCondition("inputType", (int)UIType.Direction)]
    public bool executeOnQuickTap;
    public UIIndicatorType indicatorType;
    public GameObject projectilePrefab;
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
    public List<VfxStep> startVfxList;
    public List<VfxStep> vfxList;
    public List<AnimationStep> animationList;

    // public bool Equals(SkillConfigItem skillConfigItem)
    // {
    //     return this.name.ToLower() == skillConfigItem.Name.ToLower();
    // }

    public void InitWithBackend()
    {
        // Issue #1419
        this.damage = 0;
        this.cooldown = 0f;
        this.skillRange = 0;
        this.skillCircleRadius = 5;
        // if (ServerConnection.Instance != null)
        // {
        //     foreach (var skill in ServerConnection.Instance.engineServerSettings.Skills)
        //     {
        //         var regexName = Regex.Replace(this.name, "[^0-9A-Za-z _-]", "");
        //         if (regexName.ToLower() == skill.Name.ToLower())
        //         {
        //             this.damage = 0;
        //             this.cooldown = skill.CooldownMs / 1000;
        //             this.skillRange = 0;
        //             this.skillCircleRadius = 10;
        //         }
        //     }
        // }
    }
}
