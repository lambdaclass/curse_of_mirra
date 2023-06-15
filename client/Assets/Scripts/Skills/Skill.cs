using UnityEngine;
using System.Collections;
using MoreMountains.TopDownEngine;

public class Skill : CharacterAbility
{
    [SerializeField] protected string skillId;
    [SerializeField] protected Action serverSkill;

    public void SetSkill(Action serverSkill){
        this.serverSkill = serverSkill;
    }

    public void ExecuteSkill(){
        // FIXME: Position should be removed
        RelativePosition testingPosition = new RelativePosition
        {
            X = (long)(-30 * 100),
            Y = (long)(0)
        };

        ClientAction action = new ClientAction { Action = serverSkill, Position = testingPosition };
        SocketConnectionManager.Instance.SendAction(action);
    }

    public void ExecuteSkill(Vector2 position){
        RelativePosition relativePosition = new RelativePosition
        {
            X = (long)(position.x * 100),
            Y = (long)(position.y * 100)
        };

        ClientAction action = new ClientAction { Action = serverSkill, Position = relativePosition };
        SocketConnectionManager.Instance.SendAction(action);
    }
}
