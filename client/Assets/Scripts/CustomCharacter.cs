using System.Collections;
using System.Collections.Generic;
using Communication.Protobuf;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class CustomCharacter : Character
{
    [Header("Character Base")]
    [SerializeField]
    public CharacterBase characterBase;
    public List<OldActionTracker> currentActions = new List<OldActionTracker>();

    protected override void Initialization()
    {
        base.Initialization();
        if (SocketConnectionManager.Instance.playerId.ToString() == this.PlayerID)
        {
            this.characterBase.gameObject.AddComponent<AudioSource>();
        }
    }
}
