using UnityEngine;
using MoreMountains.TopDownEngine;


public class CustomWeaponAim3D : WeaponAim3D
{
    public override void GetScriptAim()
    {
        CustomInputManager _cim = (CustomInputManager)_weapon.Owner.LinkedInputManager;

        _direction = new Vector3(_cim.customInputPosition.x, 0,_cim.customInputPosition.y);
        _currentAim = _direction;
        _weaponAimCurrentAim = _direction;
    }
}
