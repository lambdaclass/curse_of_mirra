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
        // FIXME: Position should be removed
        RelativePosition testingPosition = new RelativePosition
        {
            X = (long)(-30 * 100),
            Y = (long)(0)
        };

        ClientAction action = new ClientAction { Action = serverAbility, Position = testingPosition };
        SocketConnectionManager.Instance.SendAction(action);
    }

    public void ExecuteAbility(Vector2 position){
        RelativePosition relativePosition = new RelativePosition
        {
            X = (long)(position.x * 100),
            Y = (long)(position.y * 100)
        };

        ClientAction action = new ClientAction { Action = serverAbility, Position = relativePosition };
        SocketConnectionManager.Instance.SendAction(action);
    }
}
