using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomMMTouchJoystick : MMTouchJoystick
{
    public UnityEvent<Vector2, Skill> newPointerUpEvent;
    public UnityEvent<Vector2, CustomMMTouchJoystick> newDragEvent;
    public UnityEvent<CustomMMTouchJoystick> newPointerDownEvent;
    public Skill skill;
    float scaleCanvas;
    bool cancelState = false;
    bool dragged = false;
    bool cancelEnable = false;
    bool previousDrag = false;
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
        newDragEvent.Invoke(RawValue, this);
        dragged = true;
        CancelAttack(RawValue, dragged);
    }

    public override void OnPointerUp(PointerEventData data)
    {
        newPointerUpEvent.Invoke(RawValue, skill);
        dragged = false;
        CancelAttack(RawValue, dragged);
        UnSetJoystick();
        ResetJoystick();
    }

    public override void RefreshMaxRangeDistance()
    {
        // What makes this responsive is taking into account the canvas scaling
        scaleCanvas = GetComponentInParent<Canvas>().gameObject.transform.localScale.x;

        float knobBackgroundRadius =
            gameObject.transform.parent.GetComponent<RectTransform>().rect.width / 2;
        float knobRadius = GetComponent<RectTransform>().rect.width / 2;
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

    public void CancelAttack(Vector2 value, bool dragged)
    {
        if ((value.x < 0.7f || value.x > -0.7f) && (value.y < 0.7f || value.y > -0.7f))
        {
            cancelEnable = true;
        }
        if (dragged && cancelEnable)
        {
            if (value.x < 0.1f && value.x > -0.1f && value.y < 0.1f && value.y > -0.1f)
            {
                //Debug.Log("cancelStyles");
                previousDrag = true;
                inputManager.SetCanceled(true);
            }
            else
            {
                previousDrag = false;
                cancelEnable = false;
                //Debug.Log("no cancelStyles");
                inputManager.SetCanceled(false);
            }
        }
        if (previousDrag && !dragged)
        {
            //Debug.Log("cancel");
            inputManager.SetCanceled(true);
        }
    }
}
