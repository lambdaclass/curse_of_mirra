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
        Vector3 direction = this.GetComponent<Character>().GetComponent<CharacterOrientation3D>().ForcedRotationDirection;

        RelativePosition relativePosition = new RelativePosition
        {
            X = (long)(direction.x * 100),
            Y = (long)(direction.z * 100)
        };

        ClientAction action = new ClientAction { Action = serverSkill, Position = relativePosition };
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
