using UnityEngine;
using System.Collections;
using MoreMountains.TopDownEngine;

public class Ability : CharacterAbility
{
    [SerializeField] protected string abilityId;
    [SerializeField] protected Action serverAbility;

    public void SetAbility(Action serverAbility){
        this.serverAbility = serverAbility;
    }

    public void ExecuteAbility(){
        Debug.Log("Tap Ability executed!");

        ClientAction action = new ClientAction { Action = serverAbility };
        SocketConnectionManager.Instance.SendAction(action);
    }

    public void ExecuteAbility(Vector2 position){
        Debug.Log("Ability executed!");

        RelativePosition relativePosition = new RelativePosition
        {
            X = (long)(position.x * 100),
            Y = (long)(position.y * 100)
        };

        ClientAction action = new ClientAction { Action = serverAbility, Position = relativePosition };
        SocketConnectionManager.Instance.SendAction(action);
    }
}
