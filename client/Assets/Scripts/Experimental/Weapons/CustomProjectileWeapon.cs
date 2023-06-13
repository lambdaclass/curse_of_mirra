using UnityEngine;
using MoreMountains.TopDownEngine;
using System;

public class CustomProjectileWeapon : ProjectileWeapon
{
    public override void WeaponUse()
    {
        base.WeaponUse();
        SendActionToServer();
    }

    private void SendActionToServer()
    {
        CustomInputManager _cim = (CustomInputManager)Owner.LinkedInputManager;

        RelativePosition relativePosition = new RelativePosition
        {
            X = (long)(_cim.customInputPosition.y * -100),
            Y = (long)(_cim.customInputPosition.x * 100)
        };

        ClientAction action = new ClientAction { Action = Action.AttackAoe, Position = relativePosition };
        SocketConnectionManager.Instance.SendAction(action);
    }
}
