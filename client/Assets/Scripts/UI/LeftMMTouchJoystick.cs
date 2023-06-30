using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class LeftMMTouchJoystick : MMTouchRepositionableJoystick
{
    float positionX;
    float positionY;
    protected override void Start()
    {
        base.Start();
        _initialPosition = BackgroundCanvasGroup.transform.position;
        Input.multiTouchEnabled = false;
    }
    private Vector3 clampJoystickPositionToScreen(PointerEventData eventData)
    {
        if (eventData.position.y < GetComponent<RectTransform>().position.y + BackgroundCanvasGroup.GetComponent<RectTransform>().sizeDelta.y / 2)
        {
            positionY = eventData.position.y + BackgroundCanvasGroup.GetComponent<RectTransform>().sizeDelta.y / 2;
        }
        else
        {
            positionY = eventData.position.y;
        }
        if (eventData.position.x < GetComponent<RectTransform>().position.x + BackgroundCanvasGroup.GetComponent<RectTransform>().sizeDelta.x / 2)
        {
            positionX = eventData.position.x + BackgroundCanvasGroup.GetComponent<RectTransform>().sizeDelta.x / 2;
        }
        else
        {
            positionX = eventData.position.x;
        }
        _newPosition = new Vector3(positionX, positionY, 0f);
        return _newPosition;
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        clampJoystickPositionToScreen(eventData);
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
