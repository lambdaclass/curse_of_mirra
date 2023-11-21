using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using System.Text.RegularExpressions;

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
    public bool instantiateVfxOnModel;
    public float animationSpeedMultiplier;
    public bool hasModelAnimation;
    public AudioClip abilityStartSfx;

    public bool sfxHasAbilityStop;

    [MMCondition("sfxHasAbilityStop", true)]
    public AudioClip abilityStopSfx;
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
    public float skillRange;
    public Sprite skillSprite;

    [Header("Feedbacks")]
    public GameObject feedbackVfx;
    public float feedbackVfxDuration;
    public float feedbackVfxDelay;
    public GameObject startFeedbackVfx;
    public float startFeedbackVfxDuration;
    public float startFeedbackVfxDelay;
    public GameObject animationVfx;

    public bool Equals(SkillConfigItem skillConfigItem)
    {
        return this.name.ToLower() == skillConfigItem.Name.ToLower();
    }

    public void InitWithBackend()
    {
        if (LobbyConnection.Instance != null)
        {
            foreach (var skill in LobbyConnection.Instance.engineServerSettings.Skills)
            {
                var regexName = Regex.Replace(this.name, "[^0-9A-Za-z _-]", "");
                if (regexName.ToLower() == skill.Name.ToLower())
                {
                    this.damage = 0;
                    this.cooldown = skill.CooldownMs / 1000;
                    this.skillRange = 0;
                }
            }
        }
    }
}
