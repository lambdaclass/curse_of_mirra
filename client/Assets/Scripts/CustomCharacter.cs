using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Communication.Protobuf;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class CustomCharacter : Character
{
    [Header("Character Base")]
    [SerializeField]
    public CharacterBase characterBase;
    private HashSet<OldActionTracker> currentActions = new HashSet<OldActionTracker>();

    protected override void Initialization()
    {
        base.Initialization();
        if (GameServerConnectionManager.Instance.playerId.ToString() == this.PlayerID)
        {
            this.characterBase.gameObject.AddComponent<AudioSource>();
        }
    }

    public void rotatePlayer(RelativePosition direction)
    {
        CharacterOrientation3D characterOrientation = GetComponent<CharacterOrientation3D>();
        characterOrientation.ForcedRotation = true;
        Vector3 movementDirection = new Vector3(direction.X, 0f, direction.Y);
        movementDirection.Normalize();
        characterOrientation.ForcedRotationDirection = movementDirection;
    }

    public HashSet<OldActionTracker> GetCurrentAction()
    {
        return this.currentActions;
    }

    public void AddToCurrentActions(OldActionTracker currentAction)
    {
        currentActions.Add(currentAction);
    }

    public void RemoveFromCurrentActions(OldActionTracker currentAction)
    {
        currentActions.Remove(currentAction);
    }

    public void RotateCharacterOrientation()
    {
        this.characterBase.OrientationIndicator.transform.rotation = this.CharacterModel
            .transform
            .rotation;
    }

    public void SetPlayerDead()
    {
        CharacterFeedbacks playerFeedback = this.GetComponent<CharacterFeedbacks>();
        playerFeedback.PlayDeathFeedback();
        playerFeedback.ClearAllFeedbacks(this.gameObject);
        this.CharacterModel.SetActive(false);
        this.ConditionState.ChangeState(CharacterStates.CharacterConditions.Dead);
        this.characterBase.Hitbox.SetActive(false);
        DestroySkillsClone();
        this.GetComponentInChildren<CharacterBase>().OrientationIndicator.SetActive(false);
        //Currently unused code
        // if (GameServerConnectionManager.Instance.playerId == ulong.Parse(this.PlayerID))
        // {
        //     CustomGUIManager.DisplayZoneDamageFeedback(false);
        // }
    }

    private void DestroySkillsClone()
    {
        GetComponentsInChildren<Skill>()
            .ToList()
            .ForEach(skillInfo => Destroy(skillInfo.GetSkillInfo()));
    }

    public void HandlePlayerHealth(OldPlayer playerUpdate)
    {
        Health healthComponent = this.GetComponent<Health>();
        CharacterFeedbacks characterFeedbacks = this.GetComponent<CharacterFeedbacks>();

        characterFeedbacks.DamageFeedback(
            healthComponent.CurrentHealth,
            playerUpdate.Health,
            playerUpdate.Id
        );

        if (playerUpdate.Health != healthComponent.CurrentHealth)
        {
            healthComponent.SetHealth(playerUpdate.Health);
        }
    }
}
