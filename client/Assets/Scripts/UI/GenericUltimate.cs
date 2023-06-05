using UnityEngine;

namespace MoreMountains.TopDownEngine // you might want to use your own namespace here
{
    public class GenericUltimateAttack : CharacterAbility
    {
        GameObject ultimate;
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
            ultimate = Instantiate(Resources.Load("ultimate", typeof(GameObject))) as GameObject;
            //Set the prefab as a player child
            ultimate.transform.parent = transform;
            //Set its position to the player position
            ultimate.transform.position = transform.position;

            //Set scales
            area = ultimate.GetComponent<UltimateHandler>().area;
            area.transform.localScale = area.transform.localScale * 30;
            indicator = ultimate.GetComponent<UltimateHandler>().indicator;
            indicator.transform.localScale = indicator.transform.localScale * 5;
        }
        public void AimUltimate(Vector2 ultimatePosition)
        {
            indicator.transform.position = transform.position + new Vector3(ultimatePosition.x * 12, 0f, ultimatePosition.y * 12);
        }
        public void ExecuteUltimate(Vector2 ultimatePosition)
        {
            //Destroy ultimate animation after showing it
            Destroy(ultimate, 2.1f);

            indicator.transform.position = transform.position + new Vector3(ultimatePosition.x * 12, 0f, ultimatePosition.y * 12);
            Destroy(indicator, 0.01f);

            attack = ultimate.GetComponent<UltimateHandler>().ultimate;
            ultimate.transform.position = transform.position + new Vector3(ultimatePosition.x * 12, 0f, ultimatePosition.y * 12);
            attack.SetActive(true);

            RelativePosition relative_position = new RelativePosition
            {
                X = (long)(-ultimatePosition.y * 100),
                Y = (long)(ultimatePosition.x * 100)
            };
            ClientAction action = new ClientAction { Action = Action.Teleportation, Position = relative_position };
            SocketConnectionManager.Instance.SendAction(action);
        }
    }
}
