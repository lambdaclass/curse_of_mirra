using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class CustomCharacter : Character
{
    [Header("Character Base")]
    [SerializeField]
    public CharacterBase characterBase;
    public HashSet<PlayerAction> currentActions = new HashSet<PlayerAction>();

    private bool isTeleporing = false;

    public bool IsTeleporing {
        get { return isTeleporing; }
        set { isTeleporing = value; }
    }
    private Position teleportingDestination;
    
    public Position TeleportingDestination {
        get { return teleportingDestination; }
        set { teleportingDestination = value; }
    }
    
    CharacterFeedbacks characterFeedbacks;

    protected override void Initialization()
    {
        base.Initialization();
        if (GameServerConnectionManager.Instance.playerId.ToString() == this.PlayerID)
        {
            this.characterBase.gameObject.AddComponent<AudioSource>();
        }
        characterFeedbacks = this.GetComponent<CharacterFeedbacks>();
    }

    public void RotatePlayer(Direction direction)
    {
        CharacterOrientation3D characterOrientation = this.GetComponent<CharacterOrientation3D>();
        characterOrientation.ForcedRotation = true;
        Vector3 movementDirection = new Vector3(direction.X, 0f, direction.Y);
        movementDirection.Normalize();
        characterOrientation.ForcedRotationDirection = movementDirection;
    }

    public void SetPlayerDead()
    {
        characterFeedbacks.PlayDeathFeedback();
        characterFeedbacks.ClearAllFeedbacks(this.gameObject);
        this.CharacterModel.SetActive(false);
        this.ConditionState.ChangeState(CharacterStates.CharacterConditions.Dead);
        this.characterBase.Hitbox.SetActive(false);
        DestroySkillsClone();
        this.characterBase.CanvasHolder.SetActive(false);
    }

    private void DestroySkillsClone()
    {
        GetComponentsInChildren<Skill>()
            .ToList()
            .ForEach(skillInfo => Destroy(skillInfo.GetSkillInfo()));
    }

    public void RotateCharacterOrientation()
    {
        this.characterBase.OrientationIndicator.transform.rotation = this.CharacterModel
            .transform
            .rotation;
    }

    public void HandlePlayerHealth(Entity playerUpdate)
    {
        Health healthComponent = this.GetComponent<Health>();
        CharacterFeedbacks characterFeedbacks = this.GetComponent<CharacterFeedbacks>();

        characterFeedbacks.DamageFeedback(
            healthComponent.CurrentHealth,
            playerUpdate.Player.Health,
            playerUpdate.Id
        );

        if (playerUpdate.Player.Health != healthComponent.CurrentHealth)
        {
            healthComponent.SetHealth(playerUpdate.Player.Health);
        }
    }

    public void UpdatePowerUpsCount(ulong powerUpCount)
    {
        this.characterBase.SetPowerUpCount(powerUpCount);
    }

    public void HandleHit(float damage)
    {
        characterFeedbacks.PlayHitFeedback();
    }

    public void HandleTeleport(Position serverPosition)
    {
        if(
            this.IsTeleporing && 
            this.TeleportingDestination.X == serverPosition.X && 
            this.TeleportingDestination.Y == serverPosition.Y
        )
        {
            this.IsTeleporing = false;
            this.transform.position =
                new Vector3
                (
                    serverPosition.X / 100, 
                    this.transform.position.y,
                    serverPosition.Y / 100
                );
        }
    }
}
