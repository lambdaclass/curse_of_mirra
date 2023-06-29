using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class LeftMMTouchJoystick : MMTouchRepositionableJoystick
{
    protected override void Start()
    {
        base.Start();
        _initialPosition = BackgroundCanvasGroup.transform.position;
        Input.multiTouchEnabled = false;
    }
    private Vector3 clampedJoystickPositionToScreen(PointerEventData eventData)
    {
        if (eventData.position.y < GetComponent<RectTransform>().position.y + BackgroundCanvasGroup.GetComponent<RectTransform>().sizeDelta.y / 2)
        {
            _newPosition = new Vector3(eventData.position.x, eventData.position.y + BackgroundCanvasGroup.GetComponent<RectTransform>().sizeDelta.y / 2, 0f);
        }
        else
        {
            _newPosition = eventData.position;
        }
        return _newPosition;
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        clampedJoystickPositionToScreen(eventData);
        BackgroundCanvasGroup.transform.position = _newPosition;
        KnobCanvasGroup.GetComponent<MMTouchJoystick>().SetNeutralPosition(_newPosition);
        KnobCanvasGroup.GetComponent<MMTouchJoystick>().OnPointerDown(eventData);

    }
    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        KnobCanvasGroup.GetComponent<MMTouchJoystick>().OnDrag(eventData);

    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        if (ResetPositionToInitialOnRelease)
        {
            BackgroundCanvasGroup.transform.position = _initialPosition;
            KnobCanvasGroup.GetComponent<MMTouchJoystick>().SetNeutralPosition(_initialPosition);
            KnobCanvasGroup.GetComponent<MMTouchJoystick>().OnPointerUp(eventData);
        }
    }
}
