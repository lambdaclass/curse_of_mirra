using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomMMTouchJoystick : MoreMountains.Tools.MMTouchJoystick
{
    public UnityEvent<Vector2, Ability> newPointerUpEvent;
    public UnityEvent<Vector2> newDragEvent;
    public UnityEvent newPointerDownEvent;
    public UnityEvent<Ability> newPointerTapEvent;
    public Ability ability;

    public override void OnPointerDown(PointerEventData data)
    {
        base.OnPointerDown(data);
        SetJoystick();
        newPointerDownEvent.Invoke();
    }
    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        newDragEvent.Invoke(RawValue);
    }
    public override void OnPointerUp(PointerEventData data)
    {
        newPointerUpEvent.Invoke(RawValue, ability);
        UnSetJoystick();
        ResetJoystick();
    }
    public void OnTap(PointerEventData data)
    {
        newPointerTapEvent.Invoke(ability);
    }

    public void SetJoystick()
    {
        Image joystickBg = gameObject.transform.parent.gameObject.GetComponent<Image>();
        joystickBg.enabled = true;
    }
    public void UnSetJoystick()
    {
        Image joystickBg = gameObject.transform.parent.gameObject.GetComponent<Image>();
        joystickBg.enabled = false;
    }
}
