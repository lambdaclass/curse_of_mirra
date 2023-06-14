using UnityEngine;

namespace MoreMountains.TopDownEngine // you might want to use your own namespace here
{
    public class GenericUltimate : CharacterAbility
    {
        GameObject areaWithAim;
        GameObject area;
        GameObject indicator;
        protected override void Initialization()
        {
            base.Initialization();
        }

        public override void ProcessAbility()
        {
            base.ProcessAbility();
        }
        public void ShowAimUltimate()
        {
            //Load the prefab
            areaWithAim = Instantiate(Resources.Load("AreaAim", typeof(GameObject))) as GameObject;
            //Set the prefab as a player child
            areaWithAim.transform.parent = transform;
            //Set its position to the player position
            areaWithAim.transform.position = transform.position;

            //Set scales
            area = areaWithAim.GetComponent<AimHandler>().area;
            area.transform.localScale = area.transform.localScale * 30;
            indicator = areaWithAim.GetComponent<AimHandler>().indicator;
            indicator.transform.localScale = indicator.transform.localScale * 5;
        }
        public void AimUltimate(Vector2 ultimatePosition)
        {
            //Multiply vector values according to the scale of the animation (in this case 12)
            indicator.transform.position = transform.position + new Vector3(ultimatePosition.x * 12, 0f, ultimatePosition.y * 12);
        }
        public void ExecuteUltimate(Vector2 ultimatePosition)
        {
            //Destroy ultimate animation after showing it
            Destroy(areaWithAim, 2.1f);

            indicator.transform.position = transform.position + new Vector3(ultimatePosition.x * 12, 0f, ultimatePosition.y * 12);
            Destroy(indicator, 0.01f);
            Destroy(area, 0.01f);

            RelativePosition relative_position = new RelativePosition
            {
                X = (long)(-ultimatePosition.y * 100),
                Y = (long)(ultimatePosition.x * 100)
            };
            ClientAction action = new ClientAction { Action = Action.Teleport, Position = relative_position };
            SocketConnectionManager.Instance.SendAction(action);
            
            transform.position = new Vector3(transform.position.x + ultimatePosition.x * 12, 0, transform.position.z + ultimatePosition.y * 12);

            print("GenericUltimate call");
        }
    }
}
