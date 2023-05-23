using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CustomMMTouchJoystick : MoreMountains.Tools.MMTouchJoystick
{
    public UnityEvent<Vector2> newPointerEvent;

    public override void OnPointerUp(PointerEventData data)
    {
        newPointerEvent.Invoke(RawValue);
        ResetJoystick();
    }
}
