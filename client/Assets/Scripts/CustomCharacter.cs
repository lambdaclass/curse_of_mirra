using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class CustomCharacter : Character
{
    [Header("Character Base")]
    [SerializeField]
    public CharacterBase characterBase;
    // public HashSet<OldActionTracker> currentActions = new HashSet<OldActionTracker>();

    // protected override void Initialization()
    // {
    //     base.Initialization();
    //     if (GameServerConnectionManager.Instance.playerId.ToString() == this.PlayerID)
    //     {
    //         this.characterBase.gameObject.AddComponent<AudioSource>();
    //     }
    // }
}
