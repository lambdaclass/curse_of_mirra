using CandyCoded.HapticFeedback;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class CustomMMTouchJoystick : MMTouchJoystick
{
    public UnityEvent<Vector2, Skill> newPointerUpEvent;
    public UnityEvent<Vector2, CustomMMTouchJoystick> newDragEvent;
    public UnityEvent<CustomMMTouchJoystick> newPointerDownEvent;
    public Skill skill;
    const float CANCEL_AREA_VALUE = 0.15f;
    bool dragged = false;
    bool cancelable = false;
    private CustomInputManager inputManager;

    public override void Initialize()
    {
        base.Initialize();
        inputManager = TargetCamera.GetComponent<CustomInputManager>();
    }

    public override void OnPointerDown(PointerEventData data)
    {
        base.OnPointerDown(data);
        SetJoystick();
        newPointerDownEvent.Invoke(this);
        FirstLayer();
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        if (
            RawValue.x < CANCEL_AREA_VALUE
            && RawValue.x > -CANCEL_AREA_VALUE
            && RawValue.y < CANCEL_AREA_VALUE
            && RawValue.y > -CANCEL_AREA_VALUE
        )
        {
            cancelable = true;
            dragged = false;
        }
        else
        {
            cancelable = false;
            dragged = true;
        }
        CancelAttack();
        newDragEvent.Invoke(RawValue, this);
    }

    public override void OnPointerUp(PointerEventData data)
    {
        newPointerUpEvent.Invoke(RawValue, skill);
        cancelable = false;
        dragged = false;
        CancelAttack();
        UnSetJoystick();
        ResetJoystick();
    }

    public override void RefreshMaxRangeDistance()
    {
        // What makes this responsive is taking into account the canvas scaling
        float scaleCanvas = GetComponentInParent<Canvas>().transform.localScale.x;

        float knobBackgroundRadius = gameObject
            .transform
            .parent
            .GetComponent<RectTransform>()
            .rect
            .width;
        float knobRadius = GetComponent<RectTransform>().rect.width;
        MaxRange = (knobBackgroundRadius - knobRadius) * scaleCanvas;
        base.RefreshMaxRangeDistance();
    }

    public void FirstLayer()
    {
        Image joystickBg = gameObject.transform.parent.gameObject.GetComponent<Image>();
        joystickBg.transform.SetAsLastSibling();
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

    public void CancelAttack()
    {
        if (
            RawValue.x < CANCEL_AREA_VALUE
            && RawValue.x > -CANCEL_AREA_VALUE
            && RawValue.y < CANCEL_AREA_VALUE
            && RawValue.y > -CANCEL_AREA_VALUE
        )
        {
            inputManager.SetCanceled(cancelable, dragged, skill.GetIndicatorType());
            HapticFeedback.MediumFeedback();
        }
        else
        {
            inputManager.SetCanceled(cancelable, dragged, skill.GetIndicatorType());
        }
    }
}
