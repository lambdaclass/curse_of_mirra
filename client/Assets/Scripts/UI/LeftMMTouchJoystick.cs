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
    const float initialJoystickOpacity = 0.3f;
    const float pressedJoystickOpacity = 0.4f;

    protected override void Start()
    {
        base.Start();
        _initialPosition = BackgroundCanvasGroup.transform.position;
    }

    private Vector3 ClampJoystickPositionToScreen(PointerEventData eventData)
    {
        if (
            eventData.position.y
            < GetComponent<RectTransform>().position.y
                + BackgroundCanvasGroup.GetComponent<RectTransform>().sizeDelta.y / 2
        )
        {
            positionY =
                eventData.position.y
                + BackgroundCanvasGroup.GetComponent<RectTransform>().sizeDelta.y / 2;
        }
        else
        {
            positionY = eventData.position.y;
        }
        if (
            eventData.position.x
            < GetComponent<RectTransform>().position.x
                + BackgroundCanvasGroup.GetComponent<RectTransform>().sizeDelta.x / 2
        )
        {
            positionX =
                eventData.position.x
                + BackgroundCanvasGroup.GetComponent<RectTransform>().sizeDelta.x / 2;
        }
        else
        {
            positionX = eventData.position.x;
        }
        _newPosition = new Vector3(positionX, positionY, 0f);
        return _newPosition;
    }

    public void SetOpacity(float opacity)
    {
        BackgroundCanvasGroup.alpha = opacity;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        ClampJoystickPositionToScreen(eventData);
        BackgroundCanvasGroup.transform.position = _newPosition;
        KnobCanvasGroup.GetComponent<MMTouchJoystick>().SetNeutralPosition(_newPosition);
        KnobCanvasGroup.GetComponent<MMTouchJoystick>().OnPointerDown(eventData);
        SetOpacity(pressedJoystickOpacity);
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
        SetOpacity(initialJoystickOpacity);
    }
}
