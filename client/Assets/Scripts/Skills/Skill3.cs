using MoreMountains.TopDownEngine;

public class Skill3 : Skill
{
    protected override void Initialization(){
        base.Initialization();
        skillId = "Skill3";
    }

    public void HandleTeleport(Entity entity){
        if(skillInfo.isTeleport){
            print(entity.Position);
        }
    }
}
