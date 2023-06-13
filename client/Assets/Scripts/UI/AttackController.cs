using UnityEngine;
using System.Linq;
namespace MoreMountains.TopDownEngine // you might want to use your own namespace here
{
    /// <summary>
    /// TODO_DESCRIPTION
    /// </summary>
    [AddComponentMenu("TopDown Engine/Character/Abilities/AttackController")]
    public class AttackController : CharacterAbility
    {
        Animator animator;
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

        /// <summary>
        /// Called at the start of the ability's cycle, this is where you'll check for input
        /// </summary>
        protected override void HandleInput()
        {
            // here as an example we check if we're pressing down
            // on our main stick/direction pad/keyboard
            if (_inputManager.SecondaryMovement.y < -_inputManager.Threshold.y ||
             _inputManager.SecondaryMovement.y > -_inputManager.Threshold.y)
            {
                //SwordAttack();
            }

        }

        /// <summary>
        /// If we're pressing down, we check for a few conditions to see if we can perform our action
        /// </summary>
        public virtual void SwordAttack(bool approvedAction)
        {
            animator = this.GetComponent<Character>().CharacterModel.GetComponent<Animator>();
            // if the ability is not permitted
            if (!AbilityPermitted
                // or if we're not in our normal stance
                || (_condition.CurrentState != CharacterStates.CharacterConditions.Normal)
                // or if we're grounded
                || (!_controller.Grounded))
            {
                // we do nothing and exit
                //print("no");
                return;
            }
            animator.SetBool("ApprovedAttack", approvedAction);

            //MMDebug.DebugLogTime("animator " + animator);
        }
    }
}
