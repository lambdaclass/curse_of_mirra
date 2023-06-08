using UnityEngine;

namespace MoreMountains.TopDownEngine // you might want to use your own namespace here
{
    public class GenericAoeAttack : CharacterAbility
    {
        GameObject areaWithAim;
        GameObject area;
        GameObject indicator;
        GameObject sword;
        GameObject swordArea;
        protected override void Initialization()
        {
            base.Initialization();
        }

        /// <summary>
        /// Every frame, we check if we're crouched and if we still should be
        /// </summary>
        public override void ProcessAbility()
        {
            base.ProcessAbility();
        }
        public void ShowAimAoeAttack()
        {
            //print("down");
            //Load the prefab
            areaWithAim = Instantiate(Resources.Load("AreaAim", typeof(GameObject))) as GameObject;
            //Set the prefav as a player child
            areaWithAim.transform.parent = transform;
            //Set its position to the player position
            areaWithAim.transform.position = transform.position;

            //Set scales
            area = areaWithAim.GetComponent<AimHandler>().area;
            area.transform.localScale = area.transform.localScale * 30;
            indicator = areaWithAim.GetComponent<AimHandler>().indicator;
            indicator.transform.localScale = indicator.transform.localScale * 5;
        }
        public void AimAoeAttack(Vector2 aoePosition)
        {
            //print("drag: " + aoePosition);
            //Multiply vector values according to the scale of the animation (in this case 12)
            indicator.transform.position = transform.position + new Vector3(aoePosition.x * 12, 0f, aoePosition.y * 12);
        }
        public void ExecuteAoeAttack(Vector2 aoePosition)
        {
            //print("ability at: " + aoePosition);
            //Destroy attack animation after showing it
            Destroy(areaWithAim, 2.1f);

            indicator.transform.position = transform.position + new Vector3(aoePosition.x * 12, 0f, aoePosition.y * 12);
            Destroy(indicator, 0.01f);
            Destroy(area, 0.01f);

            sword = Instantiate(Resources.Load("Sword", typeof(GameObject))) as GameObject;
            sword.transform.position = transform.position + new Vector3(aoePosition.x * 12, 0f, aoePosition.y * 12);

            swordArea = sword.GetComponent<SwordHandler>().area;
            Destroy(swordArea, 2.1f);
            swordArea.transform.localScale = swordArea.transform.localScale * 5;

            RelativePosition relative_position = new RelativePosition
            {
                X = (long)(aoePosition.x * 100),
                Y = (long)(aoePosition.y * 100)
            };
            ClientAction action = new ClientAction { Action = Action.AttackAoe, Position = relative_position };
            SocketConnectionManager.Instance.SendAction(action);
            Destroy(sword, 2.2f);
        }

        public void ShowAoeAttack(Vector2 aoePosition)
        {
            sword = Instantiate(Resources.Load("Sword", typeof(GameObject))) as GameObject;
            sword.transform.position = new Vector3(aoePosition.x, 0.8f, aoePosition.y);

            swordArea = sword.GetComponent<SwordHandler>().area;
            Destroy(swordArea, 2.1f);
            swordArea.transform.localScale = swordArea.transform.localScale * 5;
        }
    }
}
