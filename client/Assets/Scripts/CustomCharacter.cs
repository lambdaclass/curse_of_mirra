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

    [SerializeField]
    GameObject modelAnchor;
    public HashSet<PlayerAction> currentActions = new HashSet<PlayerAction>();

    private bool isTeleporting = false;

    public bool IsTeleporting
    {
        get { return isTeleporting; }
        set { isTeleporting = value; }
    }
    private Position teleportingDestination;

    public Position TeleportingDestination
    {
        get { return teleportingDestination; }
        set { teleportingDestination = value; }
    }

    CharacterFeedbacks characterFeedbacks;
    Health healthComponent;

    protected override void Initialization()
    {
        base.Initialization();
        if (GameServerConnectionManager.Instance.playerId.ToString() == this.PlayerID)
        {
            this.characterBase.gameObject.AddComponent<AudioSource>();
        }
        characterFeedbacks = this.GetComponent<CharacterFeedbacks>();
        healthComponent = this.GetComponent<Health>();
    }

    public void RotatePlayer(Direction direction)
    {
        Vector3 movementDirection = new Vector3(direction.X, 0f, direction.Y);
        modelAnchor.transform.rotation = Quaternion.LookRotation(movementDirection);
    }

    public Quaternion CharacterRotation()
    {
        return modelAnchor.transform.rotation;
    }

    public void SetPlayerDead()
    {
        characterFeedbacks.PlayDeathFeedback();
        characterFeedbacks.ClearAllFeedbacks(this.gameObject);
        this.ConditionState.ChangeState(CharacterStates.CharacterConditions.Dead);
        this.gameObject.SetActive(false);
        DestroySkillsClone();
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
        if (playerUpdate.Player.Health != healthComponent.CurrentHealth)
        {
            characterFeedbacks.ExecuteHealthFeedback(
                healthComponent.CurrentHealth,
                playerUpdate.Player.Health,
                playerUpdate.Id
            );
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
        if (
            this.IsTeleporting
            && this.TeleportingDestination.X == serverPosition.X
            && this.TeleportingDestination.Y == serverPosition.Y
        )
        {
            this.IsTeleporting = false;
            this.transform.position = new Vector3(
                serverPosition.X / 100,
                this.transform.position.y,
                serverPosition.Y / 100
            );
        }
    }
}
