using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using System.Text.RegularExpressions;

[CreateAssetMenu(fileName = "New Skill Info", menuName = "CoM Skill")]
public class SkillInfo : ScriptableObject
{
    public new string name;
    public new string jsonName;
    public string description;
    public UIType inputType;

    public float angle;

    [MMEnumCondition("inputType", (int)UIType.Direction)]
    public bool executeOnQuickTap;
    public UIIndicatorType indicatorType;
    public GameObject projectilePrefab;
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
    public bool showCooldown;
    public float damage;
    public float cooldown;
    public ulong duration;

    public bool Equals(SkillConfigItem skillConfigItem)
    {
        return this.name.ToLower() == skillConfigItem.Name.ToLower();
    }

    void OnEnable()
    {
        if (LobbyConnection.Instance != null)
        {
            foreach (var skill in LobbyConnection.Instance.serverSettings.SkillsConfig.Items)
            {
                var regexName = Regex.Replace(this.name, "[^0-9A-Za-z _-]", "");
                if (regexName.ToLower() == skill.Name.ToLower())
                {
                    this.damage = float.Parse(skill.Damage);
                    this.cooldown = float.Parse(skill.Cooldown);
                    this.jsonName = skill.Name;
                    this.duration = ulong.Parse(skill.Duration);
                }
            }
        }
    }
}
