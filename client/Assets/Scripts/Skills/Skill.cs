using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;
using UnityEngine.VFX;
using static MoreMountains.Tools.MMSoundManager;

public class Skill : CharacterAbility
{
    const float MAX_RANGE = 50f;
    const float INNER_RANGE = 2.5f;

    [SerializeField]
    public string skillId;

    // [SerializeField]
    // protected Communication.Protobuf.Action serverSkill;
    protected string serverSkill;

    [SerializeField]
    protected bool blocksMovementOnExecute = true;

    [SerializeField]
    protected SkillInfo skillInfo;

    private List<ulong> usedPools = new List<ulong>();

    // feedbackRotatePosition used to track the position to look at when executing the animation feedback
    private Vector2 feedbackRotatePosition;

    StaminaManager staminaManager;

    protected override void Start()
    {
        base.Start();
        if (blocksMovementOnExecute)
        {
            BlockingMovementStates = new CharacterStates.MovementStates[1];
            BlockingMovementStates[0] = CharacterStates.MovementStates.Attacking;
        }

        if (skillInfo)
        {
            _animator.SetFloat(skillId + "Speed", skillInfo.animationSpeedMultiplier);
        }

        staminaManager = Utils
            .GetCharacter(GameServerConnectionManager.Instance.playerId)
            .characterBase
            .CharacterCard
            .GetComponentInChildren<StaminaManager>();
    }

    public void SetSkill(string serverSkill, SkillInfo skillInfo)
    {
        this.serverSkill = serverSkill;
        this.skillInfo = skillInfo;
        this.AbilityStartSfx = skillInfo.abilityStartSfx;
        if (skillInfo.sfxHasAbilityStop)
        {
            this.AbilityStopSfx = skillInfo.abilityStopSfx;
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
            Direction direction = new Direction { X = 0, Y = 0 };
            ExecuteSkill(direction);
            CheckAvailableStamina();
        }
    }

    public void TryExecuteSkill(Vector2 position)
    {
        if (AbilityAuthorized)
        {
            Direction direction = new Direction { X = position.x, Y = position.y };
            feedbackRotatePosition = new Vector2(position.x, position.y);
            ExecuteSkill(direction);
            CheckAvailableStamina();
        }
    }

    private void ExecuteSkill(Direction direction)
    {
        var player = Utils.GetGamePlayer(GameServerConnectionManager.Instance.playerId);

        if (AbilityAuthorized && player.Player.AvailableStamina >= skillInfo.staminaCost)
        {
            SendActionToBackend(direction);
        }
    }

    void CheckAvailableStamina()
    {
        var player = Utils.GetGamePlayer(GameServerConnectionManager.Instance.playerId);
        if (
            player.Player.AvailableStamina < skillInfo.staminaCost
            && !staminaManager.playingFeedback && !skillInfo.useCooldown
        )
        {
            staminaManager.UnavailableStaminaFeedback();
        }
    }

    public void ExecuteFeedbacks(ulong duration, bool blockMovement)
    {
        ClearAnimator();

        // Setup
        AudioClip sfxClip;

        sfxClip = skillInfo.sfxHasAbilityStop ? skillInfo.abilityStopSfx : null;

        // State & animation
        ChangeCharacterState(skillId, blockMovement);
        if (skillInfo.animationList.Count > 0)
        {
            List<AnimationStep> animationList = new List<AnimationStep>(skillInfo.animationList);
            StartCoroutine(ExecuteChainedAnimation(animationList, (duration / 1000f)));
        }
        StartCoroutine(AutoEndSkillAnimation(skillId, duration / 1000f));

        // Visual effects
        foreach (var vfxStep in skillInfo.vfxList)
        {
            StartCoroutine(
                ExecuteFeedbackVfx(
                    vfxStep.vfx,
                    vfxStep.duration,
                    vfxStep.delay,
                    vfxStep.instantiateVfxOnModel,
                    this.skillInfo.hasSkillPool
                )
            );
        }

        // Sound effects
        if (sfxClip)
        {
            Sound3DManager sound3DManager = GetComponentInChildren<Sound3DManager>();
            sound3DManager.SetSfxSound(sfxClip);
            sound3DManager.PlaySfxSound();
        }
    }

    public IEnumerator AutoEndSkillAnimation(string skillAnimationId, float duration)
    {
        yield return new WaitForSeconds(duration);
        EndSkillAnimation(skillAnimationId);
    }

    public void EndSkillAnimation(string animationId)
    {
        _movement.ChangeState(CharacterStates.MovementStates.Idle);
        _animator.SetBool(animationId, false);
    }

    IEnumerator ExecuteChainedAnimation(
        List<AnimationStep> pendingAnimations,
        float totalDuration,
        float previousAnimationStep = 0
    )
    {
        AnimationStep nextAnimation = pendingAnimations[0];
        pendingAnimations.RemoveAt(0);

        float animationDuration = nextAnimation.durationPercent * totalDuration;
        float animationStep = previousAnimationStep + 1;
        string animationStepId = skillId + "_s" + animationStep;

        string previousAnimationStepId = skillId + "_s" + previousAnimationStep;

        SetAnimation(previousAnimationStepId, false);
        SetAnimation(animationStepId, true);

        if (nextAnimation.triggersVfx)
        {
            StartCoroutine(
                ExecuteFeedbackVfx(
                    nextAnimation.vfxStep.vfx,
                    nextAnimation.vfxStep.duration,
                    nextAnimation.vfxStep.delay,
                    nextAnimation.vfxStep.instantiateVfxOnModel,
                    this.skillInfo.hasSkillPool
                )
            );
        }

        yield return new WaitForSeconds(animationDuration);

        if (pendingAnimations.Count > 0)
        {
            StartCoroutine(
                ExecuteChainedAnimation(pendingAnimations, totalDuration, animationStep)
            );
        }
        else
        {
            SetAnimation(animationStepId, false);
        }
    }

    IEnumerator ExecuteFeedbackVfx(
        GameObject vfx,
        float duration,
        float delay,
        bool instantiateVfxOnModel,
        bool hasSkillPool
    )
    {
        yield return new WaitForSeconds(delay);

        GameObject vfxInstance;

        if (instantiateVfxOnModel)
        {
            vfxInstance = Instantiate(vfx, _model.transform);
            vfxInstance
                .GetComponent<PinnedEffectsController>()
                ?.Setup(this.GetComponent<PinnedEffectsManager>());
        }
        else
        {
            Vector3 vfxPosition = new Vector3(
                _model.transform.position.x,
                vfx.transform.position.y,
                _model.transform.position.z
            );

            if(hasSkillPool){
               vfxPosition = SetPoolDiameterAndPosition(vfx);
            }
            vfxInstance = Instantiate(vfx, vfxPosition, vfx.transform.rotation);
           
            vfxInstance
                .GetComponent<PinnedEffectsController>()
                ?.Setup(this.GetComponent<PinnedEffectsManager>());
        }

        Destroy(vfxInstance, duration);
    }
 

    private Vector3 SetPoolDiameterAndPosition(GameObject vfx){
        float diameter = 0;
        Vector3 vfxPosition = Vector3.zero;
         GameServerConnectionManager.Instance.gamePools.ForEach(pool => {
            if(pool.Pool.OwnerId == skillInfo.ownerId && !usedPools.Contains(pool.Id)){
                vfxPosition =  Utils.transformBackendOldPositionToFrontendPosition(pool.Position);
                diameter = Utils.TransformBackenUnitToClientUnit(pool.Radius) * 2;
            }
        });

        if(vfx.transform.childCount > 0){
            if(vfx.GetComponentInChildren<VisualEffect>()){
               vfx.GetComponentInChildren<VisualEffect>().SetFloat("EffectDiameter", diameter);
            } else {
                // Placeholder, we should have the same implementation for the vfx as above
                vfx.transform.localScale = new Vector3(diameter/10, diameter/10, diameter/10); 
            }

        }

        return vfxPosition;
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

    private void SetAnimation(string animationId, bool value)
    {
        _animator.SetBool(animationId, value);
    }

    private void ChangeCharacterState(string animation, bool blockingMovement)
    {
        CharacterStates.MovementStates currentState = blockingMovement
            ? CharacterStates.MovementStates.Attacking
            : CharacterStates.MovementStates.Dashing;

        _movement.ChangeState(currentState);
        _animator.SetBool(animation, true);
    }

    private void ChangeCharacterStateToDash(string animation)
    {
        _movement.ChangeState(CharacterStates.MovementStates.Dashing);
        _animator.SetBool(animation, true);
    }

    private void SendActionToBackend(Direction direction)
    {
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        _movement.ChangeState(CharacterStates.MovementStates.Pushing);

        GameServerConnectionManager.Instance.clientPrediction.StopMovement();
        GameServerConnectionManager.Instance.SendSkill(serverSkill, direction, timestamp);
    }

    public virtual void StopAbilityStopFeedbacks()
    {
        AbilityStopFeedbacks?.StopFeedbacks();
    }

    public float GetAngle()
    {
        return this.skillInfo.angle;
    }

    public float GetSkillRange()
    {
        float range;
        switch (skillInfo.skillCircleRange)
        {
            case 0:
                range = MAX_RANGE;
                break;
            case -1:
                range = INNER_RANGE;
                break;
            default:
                range = skillInfo.skillCircleRange;
                break;
        }
        return range;
    }

    public void SetSkillRange(float range)
    {
        skillInfo.skillCircleRange = range;
    }

    public void SetSkillAreaRadius(float radius)
    {
        skillInfo.skillAreaRadius = radius;
    }

    public float GetSkillAreaRadius(){
        return skillInfo.skillAreaRadius;
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

    public bool IsSelfTargeted()
    {
        return skillInfo.skillCircleRange == -1;
    }
}
