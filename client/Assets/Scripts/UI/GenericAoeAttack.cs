using UnityEngine;

namespace MoreMountains.TopDownEngine // you might want to use your own namespace here
{
    public class GenericAoeAttack : CharacterAbility
    {
        GameObject aoeAttack;
        GameObject area;
        GameObject indicator;
        GameObject attack;
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
            aoeAttack = Instantiate(Resources.Load("AoeAttack", typeof(GameObject))) as GameObject;
            //Set the prefav as a player child
            aoeAttack.transform.parent = transform;
            //Set its position to the player position
            aoeAttack.transform.position = transform.position;

            //Set scales
            area = aoeAttack.GetComponent<AoeAttackHandler>().area;
            area.transform.localScale = area.transform.localScale * 30;
            indicator = aoeAttack.GetComponent<AoeAttackHandler>().indicator;
            indicator.transform.localScale = indicator.transform.localScale * 5;
        }
        public void AimAoeAttack(Vector2 aoePosition)
        {
            //print("drag: " + aoePosition);
            //Multiply vector values according to the scale of the animation (in this case 5)
            indicator.transform.position = transform.position + new Vector3(aoePosition.x * 12, 0f, aoePosition.y * 12);
        }
        public void ExecuteAoeAttack(Vector2 aoePosition)
        {
            //print("ability at: " + aoePosition);
            //Destroy attack animation after showing it
            Destroy(aoeAttack, 2.1f);

            indicator.transform.position = transform.position + new Vector3(aoePosition.x * 12, 0f, aoePosition.y * 12);
            Destroy(indicator, 0.01f);

            attack = aoeAttack.GetComponent<AoeAttackHandler>().attack;
            attack.transform.position = transform.position + new Vector3(aoePosition.x * 12, 0f, aoePosition.y * 12);
            attack.SetActive(true);

            RelativePosition relative_position = new RelativePosition
            {
                X = (long)(-aoePosition.y * 100),
                Y = (long)(aoePosition.x * 100)
            };
            ClientAction action = new ClientAction { Action = Action.AttackAoe, Position = relative_position };
            SocketConnectionManager.Instance.SendAction(action);
        }
    }
}
