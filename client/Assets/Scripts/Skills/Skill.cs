using System;
using System.Collections;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;
using static MoreMountains.Tools.MMSoundManager;

public class Skill : CharacterAbility
{
    const float MAX_RANGE = 50f;
    const float INNER_RANGE = 2.5f;

    [SerializeField]
    public string skillId;

    [SerializeField]
    protected Action serverSkill;

    [SerializeField]
    protected bool blocksMovementOnExecute = true;

    [SerializeField]
    protected SkillInfo skillInfo;

    protected SkillAnimationEvents skillsAnimationEvent;

    // feedbackRotatePosition used to track the position to look at when executing the animation feedback
    private Vector2 feedbackRotatePosition;
    private GameObject startFeedbackVfx;
    private GameObject feedbackVfx;
    private TrailRenderer trail;

    public void SetSkill(
        Action serverSkill,
        SkillInfo skillInfo,
        SkillAnimationEvents skillsAnimationEvent
    )
    {
        this.serverSkill = serverSkill;
        this.skillInfo = skillInfo;
        this.skillsAnimationEvent = skillsAnimationEvent;
        this.AbilityStartSfx = skillInfo.abilityStartSfx;
        if (skillInfo.sfxHasAbilityStop)
        {
            this.AbilityStopSfx = skillInfo.abilityStopSfx;
        }
    }

    protected override void Start()
    {
        base.Start();
        if (blocksMovementOnExecute)
        {
            BlockingMovementStates = new CharacterStates.MovementStates[1];
            BlockingMovementStates[0] = CharacterStates.MovementStates.Attacking;
        }

        if (skillInfo.startFeedbackVfx)
        {
            Transform animationParent;
            animationParent = skillInfo.instantiateVfxOnModel
                ? _model.transform
                : _model.transform.parent;
            startFeedbackVfx = Instantiate(skillInfo.startFeedbackVfx, animationParent);

            if (skillInfo.feedbackVfx.GetComponent<UnityEngine.VFX.VisualEffect>())
            {
                startFeedbackVfx.SetActive(false);
            }
            else if (skillInfo.feedbackVfx.GetComponent<TrailRenderer>())
            {
                trail = startFeedbackVfx.GetComponent<TrailRenderer>();
                trail.emitting = false;
            }
            else
            {
                this.AbilityStartFeedbacks = startFeedbackVfx.GetComponent<MMF_Player>();
            }
        }

        if (skillInfo.feedbackVfx)
        {
            Transform animationParent;
            animationParent = skillInfo.instantiateVfxOnModel
                ? _model.transform
                : _model.transform.parent;
            feedbackVfx = Instantiate(skillInfo.feedbackVfx, animationParent);

            if (skillInfo.feedbackVfx.GetComponent<UnityEngine.VFX.VisualEffect>())
            {
                feedbackVfx.SetActive(false);
            }
            else if (skillInfo.feedbackVfx.GetComponent<TrailRenderer>())
            {
                trail = feedbackVfx.GetComponent<TrailRenderer>();
                trail.emitting = false;
            }
            else
            {
                this.AbilityStopFeedbacks = feedbackVfx.GetComponent<MMF_Player>();
                if (skillInfo.indicatorType == UIIndicatorType.Area)
                {
                    Transform parentTransform = feedbackVfx.GetComponent<MMF_Player>().transform;
                    float diameter = skillInfo.skillAreaRadius * 2;
                    parentTransform.localScale = new Vector3(diameter, diameter, diameter);
                }
            }
        }

        if (skillInfo)
        {
            _animator.SetFloat(skillId + "Speed", skillInfo.animationSpeedMultiplier);
        }
    }

    public SkillInfo GetSkillInfo()
    {
        return skillInfo;
    }

    public GameObject GetProjectileFromSkill()
    {
        return skillInfo?.projectilePrefab;
    }

    public void TryExecuteSkill()
    {
        if (AbilityAuthorized)
        {
            Vector3 direction = this.GetComponent<Character>()
                .GetComponent<CharacterOrientation3D>()
                .ForcedRotationDirection;
            RelativePosition relativePosition = new RelativePosition
            {
                X = direction.x,
                Y = direction.z
            };
            feedbackRotatePosition = new Vector2(direction.x, direction.z);
            ExecuteSkill(relativePosition);
        }
    }

    public void TryExecuteSkill(Vector2 position)
    {
        if (AbilityAuthorized)
        {
            RelativePosition relativePosition = new RelativePosition
            {
                X = position.x,
                Y = position.y
            };
            feedbackRotatePosition = new Vector2(position.x, position.y);
            ExecuteSkill(relativePosition);
        }
    }

    private void ExecuteSkill(RelativePosition relativePosition)
    {
        bool hasMoved = relativePosition.X != 0 || relativePosition.Y != 0;
        if (AbilityAuthorized && hasMoved)
        {
            SendActionToBackend(relativePosition);
        }
    }

    public void StartFeedback()
    {
        ClearAnimator();

        if (skillInfo.hasModelAnimation == true)
        {
            string animation = skillId + "_start";
            ChangeCharacterState(animation);
            if (skillInfo.startAnimationDuration > 0)
            {
                StartCoroutine(
                    skillsAnimationEvent.TryEjectAnimation(
                        this,
                        animation,
                        skillInfo.startAnimationDuration
                    )
                );
            }
        }

        if (skillInfo.startFeedbackVfx)
        {
            StartCoroutine(StartFeedbackVfx());
        }

        if (skillInfo.sfxHasAbilityStop)
        {
            // We have to change the abilityStartSfx to abilityStopSfx when we have 2 sfx for each animation, for now this is a little hack
            GetComponentInChildren<Sound3DManager>()
                .SetSfxSound(skillInfo.abilityStartSfx);
            GetComponentInChildren<Sound3DManager>().PlaySfxSound();
        }
    }

    IEnumerator StartFeedbackVfx()
    {
        yield return new WaitForSeconds(skillInfo.startFeedbackVfxDelay);

        if (skillInfo.startFeedbackVfx.GetComponent<MMF_Player>())
        {
            this.PlayAbilityStartFeedbacks();
        }
        if (skillInfo.startFeedbackVfx.GetComponent<UnityEngine.VFX.VisualEffect>())
        {
            skillInfo.startFeedbackVfx.SetActive(true);
        }
        StartCoroutine(StopStartFeedbackVfx());
    }

    public void ExecuteFeedback()
    {
        ClearAnimator();

        if (skillInfo.hasModelAnimation == true)
        {
            ChangeCharacterState(skillId);
            if (skillInfo.executeAnimationDuration > 0)
            {
                StartCoroutine(
                    skillsAnimationEvent.TryEjectAnimation(
                        this,
                        skillId,
                        skillInfo.executeAnimationDuration
                    )
                );
            }
        }

        if (!skillInfo.sfxHasAbilityStop)
        {
            GetComponentInChildren<Sound3DManager>().SetSfxSound(skillInfo.abilityStartSfx);
            GetComponentInChildren<Sound3DManager>().PlaySfxSound();
        }

        if (skillInfo.feedbackVfx)
        {
            StartCoroutine(ExecuteFeedbackVfx());
        }
    }

    IEnumerator ExecuteFeedbackVfx()
    {
        yield return new WaitForSeconds(skillInfo.feedbackVfxDelay);

        if (skillInfo.feedbackVfx.GetComponent<MMF_Player>())
        {
            this.PlayAbilityStopFeedbacks();
        }
        if (skillInfo.feedbackVfx.GetComponent<UnityEngine.VFX.VisualEffect>())
        {
            feedbackVfx.SetActive(true);
        }
        if (trail)
        {
            trail.emitting = true;
        }
        StartCoroutine(StopFeedbackVfx());
    }

    private void ClearAnimator()
    {
        foreach (int skill in Enum.GetValues(typeof(UIControls)))
        {
            String skillName = Enum.GetName(typeof(UIControls), skill);
            _animator.SetBool(skillName, false);
            _animator.SetBool(skillName + "_start", false);
        }
    }

    private void ChangeCharacterState(string animation)
    {
        skillsAnimationEvent.UpdateActiveSkill(this, animation);
        _movement.ChangeState(CharacterStates.MovementStates.Attacking);
        _animator.SetBool(animation, true);
    }

    private void SendActionToBackend(RelativePosition relativePosition)
    {
        ClientAction action = new ClientAction
        {
            Action = serverSkill,
            Position = relativePosition
        };
        SocketConnectionManager.Instance.SendAction(action);
    }

    public void EndSkillFeedback(string animationId)
    {
        _movement.ChangeState(CharacterStates.MovementStates.Idle);
        _animator.SetBool(animationId, false);
    }

    IEnumerator StopFeedbackVfx()
    {
        yield return new WaitForSeconds(skillInfo.feedbackVfxDuration);

        if (feedbackVfx.GetComponent<MMF_Player>())
        {
            this.StopAbilityStopFeedbacks();
        }
        if (feedbackVfx.GetComponent<UnityEngine.VFX.VisualEffect>())
        {
            feedbackVfx.SetActive(false);
        }
        if (trail)
        {
            trail.emitting = false;
        }
    }

    IEnumerator StopStartFeedbackVfx()
    {
        yield return new WaitForSeconds(skillInfo.startFeedbackVfxDuration);

        if (startFeedbackVfx.GetComponent<MMF_Player>())
        {
            this.StopStartFeedbacks();
        }
        if (startFeedbackVfx.GetComponent<UnityEngine.VFX.VisualEffect>())
        {
            startFeedbackVfx.SetActive(false);
        }
        if (trail)
        {
            trail.emitting = false;
        }
    }

    public virtual void StopAbilityStopFeedbacks()
    {
        AbilityStopFeedbacks?.StopFeedbacks();
    }

    public float GetAngle()
    {
        return this.skillInfo.angle;
    }

    public float GetSkillRadius()
    {
        float radius;
        switch (skillInfo.skillCircleRadius)
        {
            case 0:
                radius = MAX_RANGE;
                break;
            case -1:
                radius = INNER_RANGE;
                break;
            default:
                radius = skillInfo.skillCircleRadius;
                break;
        }
        return radius;
    }

    public void SetSkillRadius(float radius)
    {
        skillInfo.skillCircleRadius = radius;
    }

    public void SetSkillAreaRadius(float radius)
    {
        skillInfo.skillAreaRadius = radius;
    }

    public float GetIndicatorAngle()
    {
        return skillInfo.skillConeAngle;
    }

    public float GetArroWidth()
    {
        return skillInfo.arrowWidth;
    }

    public UIIndicatorType GetIndicatorType()
    {
        return skillInfo.indicatorType;
    }

    public String GetSkillName()
    {
        return skillInfo.name;
    }

    public bool ExecutesOnQuickTap()
    {
        return skillInfo.executeOnQuickTap;
    }

    public bool IsSelfTargeted()
    {
        return skillInfo.skillCircleRadius == -1;
    }
}
