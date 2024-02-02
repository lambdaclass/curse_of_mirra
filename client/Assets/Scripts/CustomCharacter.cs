using System.Collections;
using System.Collections.Generic;
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
}
