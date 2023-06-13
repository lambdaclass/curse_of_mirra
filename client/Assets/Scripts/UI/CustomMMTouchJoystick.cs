using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomMMTouchJoystick : MoreMountains.Tools.MMTouchJoystick
{
    public UnityEvent<Vector2, Weapon> newPointerUpEvent;
    public UnityEvent<Vector2> newDragEvent;
    public UnityEvent newPointerDownEvent;
    public Weapon action;
    public override void OnPointerDown(PointerEventData data)
    {
        base.OnPointerDown(data);
        newPointerDownEvent.Invoke();
    }
    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        newDragEvent.Invoke(RawValue);
    }
    public override void OnPointerUp(PointerEventData data)
    {
        newPointerUpEvent.Invoke(RawValue, action);
        ResetJoystick();
    }
}
