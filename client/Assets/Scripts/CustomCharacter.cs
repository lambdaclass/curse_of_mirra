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

    protected override void Initialization()
    {
        base.Initialization();
        if (GameServerConnectionManager.Instance.playerId.ToString() == this.PlayerID)
        {
            this.characterBase.gameObject.AddComponent<AudioSource>();
        }
    }

    public void rotatePlayer(GameObject player, Direction direction)
    {
        CharacterOrientation3D characterOrientation = this.GetComponent<CharacterOrientation3D>();
        characterOrientation.ForcedRotation = true;
        Vector3 movementDirection = new Vector3(direction.X, 0f, direction.Y);
        movementDirection.Normalize();
        characterOrientation.ForcedRotationDirection = movementDirection;
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
        // if (GameServerConnectionManager.Instance.playerId == ulong.Parse(playerCharacter.PlayerID))
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

   public void RotateCharacterOrientation()
    {
        this.characterBase.OrientationIndicator.transform.rotation =
            this.CharacterModel.transform.rotation;
    }
}
