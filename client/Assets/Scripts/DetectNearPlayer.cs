using System;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;
public class DetectNearPlayer : MonoBehaviour
{
   public void GetPlayerFaceDirection()
    {
        Vector3 myCharacterDirection = this.GetComponent<Character>().GetComponent<CharacterOrientation3D>().ForcedRotationDirection;
        PlayerControls.BasicAttack(myCharacterDirection);
    }
}
