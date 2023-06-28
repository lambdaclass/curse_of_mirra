using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class LeftMMTouchJoystick : MMTouchJoystick
{
    public UnityEvent<Vector2> newPointerUpEvent;
    public UnityEvent<Vector2> newDragEvent;
    public UnityEvent<Vector2> newPointerDownEvent;
    [SerializeField] public MMTouchJoystick joystickL;


    public override void OnPointerDown(PointerEventData data)
    {
        base.OnPointerDown(data);
        joystickL.OnPointerDown(data);
        newPointerDownEvent.Invoke(data.position);
    }
    public override void OnDrag(PointerEventData eventData)
    {
        newDragEvent.Invoke(RawValue);
        joystickL.OnDrag(eventData);
    }
    public override void OnPointerUp(PointerEventData data)
    {
        newPointerUpEvent.Invoke(data.position);
        joystickL.OnPointerUp(data);
        base.OnPointerUp(data);
    }
}
