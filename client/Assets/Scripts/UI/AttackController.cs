using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.TopDownEngine // you might want to use your own namespace here
{
    /// <summary>
    /// TODO_DESCRIPTION
    /// </summary>
    [AddComponentMenu("TopDown Engine/Character/Abilities/AttackController")]
    public class AttackController : CharacterAbility
    {
        [SerializeField] Transform rangeImage;
        [SerializeField] Transform areaImage;
        [SerializeField] float rangeScaleNumber;
        [SerializeField] float areaScaleNumber;

        protected override void Initialization()
        {
            base.Initialization();
            rangeImage.localScale += new Vector3(rangeScaleNumber, rangeScaleNumber, rangeScaleNumber);
            areaImage.localScale += new Vector3(areaScaleNumber, areaScaleNumber, areaScaleNumber);
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
                DamageAreaMovement();
            }

        }

        /// <summary>
        /// If we're pressing down, we check for a few conditions to see if we can perform our action
        /// </summary>
        protected virtual void DamageAreaMovement()
        {
            // if the ability is not permitted
            if (!AbilityPermitted
                // or if we're not in our normal stance
                || (_condition.CurrentState != CharacterStates.CharacterConditions.Normal)
                // or if we're grounded
                || (!_controller.Grounded))
            {
                // we do nothing and exit
                print("no");
                return;
            }


            //We need the outside varibables to be added to our math since they are in charge of scaling everything, thats why we are multiplying some input values with them.

            //Need to calculate half of each area because 0;0 is in the center
            //or else the area would move entirely outside the range, sticking to the outter border
            float halfRange = rangeScaleNumber / 2;
            float halfArea = areaScaleNumber / 2;

            //Need the difference between the range and the area because that's what should be added to the movement
            //or else the area would move half way outside the range
            float diffRangeArea = halfRange - halfArea;


            // aim range is taking the character position
            // when using right joystick it adds to it
            // substracting the character position gives the actual position
            // this is needed because if not it wouldn't visually correspond to the joystic (in an absolute position in the ui), the aim area position would depend on the direction the player is in
            Vector3 characterPosition = transform.position;


            areaImage.position = characterPosition + new Vector3((_inputManager.SecondaryMovement.x * (diffRangeArea)), -0.8f, (_inputManager.SecondaryMovement.y * (diffRangeArea)));


            //MMDebug.DebugLogTime("sm " + _inputManager.SecondaryMovement);
            //MMDebug.DebugLogTime("areaImage " + areaImage.position);
            //MMDebug.DebugLogTime("character " + transform.position);
        }
    }
}
