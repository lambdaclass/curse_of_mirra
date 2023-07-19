using System.Collections;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class Skill : CharacterAbility
{
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
    private GameObject feedbackAnimation;
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
    }

    protected override void Start()
    {
        base.Start();

        if (blocksMovementOnExecute)
        {
            BlockingMovementStates = new CharacterStates.MovementStates[1];
            BlockingMovementStates[0] = CharacterStates.MovementStates.Attacking;
        }

        if (skillInfo.feedbackAnimation)
        {
            Transform animationParent;
            animationParent = skillInfo.instantiateAnimationOnModel
                ? _model.transform
                : _model.transform.parent;
            feedbackAnimation = Instantiate(skillInfo.feedbackAnimation, animationParent);

            if (skillInfo.feedbackAnimation.GetComponent<UnityEngine.VFX.VisualEffect>())
            {
                feedbackAnimation.SetActive(false);
            }
            else if (skillInfo.feedbackAnimation.GetComponent<TrailRenderer>())
            {
                trail = feedbackAnimation.GetComponent<TrailRenderer>();
                trail.emitting = false;
            }
            else
            {
                this.AbilityStartFeedbacks = feedbackAnimation.GetComponent<MMF_Player>();
            }
        }

        if (skillInfo)
        {
            _animator.SetFloat(skillId + "Speed", skillInfo.animationSpeedMultiplier);
        }
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

    public void ExecuteFeedback()
    {
        GetComponent<CharacterOrientation3D>().ForcedRotationDirection.z = feedbackRotatePosition.y;
        GetComponent<CharacterOrientation3D>().ForcedRotationDirection.x = feedbackRotatePosition.x;

        if (skillInfo.hasModelAnimation == true)
        {
            skillsAnimationEvent.UpdateActiveSkill(this);
            _movement.ChangeState(CharacterStates.MovementStates.Attacking);
            _animator.SetBool(skillId, true);
            PlayAbilityStartSfx();
        }

        if (skillInfo.feedbackAnimation)
        {
            if (skillInfo.feedbackAnimation.GetComponent<MMF_Player>())
            {
                this.PlayAbilityStartFeedbacks();
            }
            if (skillInfo.feedbackAnimation.GetComponent<UnityEngine.VFX.VisualEffect>())
            {
                feedbackAnimation.SetActive(true);
            }
            if (trail)
            {
                trail.emitting = true;
            }
        }
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

    public void EndSkillFeedback()
    {
        _movement.ChangeState(CharacterStates.MovementStates.Idle);
        _animator.SetBool(skillId, false);

        if (feedbackAnimation)
        {
            if (feedbackAnimation.GetComponent<MMF_Player>())
            {
                this.StopStartFeedbacks();
            }
            if (feedbackAnimation.GetComponent<UnityEngine.VFX.VisualEffect>())
            {
                feedbackAnimation.SetActive(false);
            }
            if (trail)
            {
                StartCoroutine(StopEmitting(.3f));
            }
        }
    }

    IEnumerator StopEmitting(float time)
    {
        yield return new WaitForSeconds(time);
        trail.emitting = false;
    }
}
