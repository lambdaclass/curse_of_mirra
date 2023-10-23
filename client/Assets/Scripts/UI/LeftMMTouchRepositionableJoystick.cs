using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class LeftMMTouchRepositionableJoystick : MMTouchRepositionableJoystick
{
    float positionX;
    float positionY;
    const float initialJoystickOpacity = 0.3f;
    const float pressedJoystickOpacity = 0.4f;
    float scaleCanvas;

    protected override void Start()
    {
        base.Start();
        scaleCanvas = GetComponentInParent<Canvas>().gameObject.transform.localScale.x;
        _initialPosition = BackgroundCanvasGroup.transform.position;
    }

    private Vector3 ClampJoystickPositionToScreen(PointerEventData eventData)
    {
        if (
            eventData.position.y
            < GetComponent<RectTransform>().position.y
                + BackgroundCanvasGroup.GetComponent<RectTransform>().sizeDelta.y / 2 * scaleCanvas
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
                + BackgroundCanvasGroup.GetComponent<RectTransform>().sizeDelta.x / 2 * scaleCanvas
        )
        {
            positionX =
                eventData.position.x
                + BackgroundCanvasGroup.GetComponent<RectTransform>().sizeDelta.x / 2 ;
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
        KnobCanvasGroup.GetComponent<LeftMMTouchJoystick>().SetNeutralPosition(_newPosition);
        KnobCanvasGroup.GetComponent<LeftMMTouchJoystick>().OnPointerDown(eventData);
        SetOpacity(pressedJoystickOpacity);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        KnobCanvasGroup.GetComponent<LeftMMTouchJoystick>().OnDrag(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        if (ResetPositionToInitialOnRelease)
        {
            BackgroundCanvasGroup.transform.position = _initialPosition;
            KnobCanvasGroup.GetComponent<LeftMMTouchJoystick>().SetNeutralPosition(_initialPosition);
            KnobCanvasGroup.GetComponent<LeftMMTouchJoystick>().OnPointerUp(eventData);
        }
        SetOpacity(initialJoystickOpacity);
    }
}
