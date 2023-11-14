using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomMMTouchRepositionableJoystick : MMTouchRepositionableJoystick
{
    float positionX;
    float positionY;
    float scaleCanvas;
    CustomMMTouchJoystick knobJoystick;
    CustomMMTouchButton knobButton;

    protected override void Start()
    {
        base.Start();
        scaleCanvas = GetComponentInParent<Canvas>().transform.localScale.x;
        _initialPosition = BackgroundCanvasGroup.transform.position;
        knobJoystick = KnobCanvasGroup.GetComponent<CustomMMTouchJoystick>();
        knobButton = KnobCanvasGroup.GetComponent<CustomMMTouchButton>();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        ClampJoystickPositionToScreen(eventData);
        if (knobJoystick.isActiveAndEnabled)
        {
            BackgroundCanvasGroup.transform.position = _newPosition;
            knobJoystick.SetNeutralPosition(_newPosition);
            knobJoystick.OnPointerDown(eventData);
        }
        knobButton.OnPointerDown(eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        if (knobJoystick.isActiveAndEnabled)
        {
            knobJoystick.OnDrag(eventData);
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        if (ResetPositionToInitialOnRelease)
        {
            if (knobJoystick.isActiveAndEnabled)
            {
                BackgroundCanvasGroup.transform.position = _initialPosition;
                knobJoystick.SetNeutralPosition(_initialPosition);
                knobJoystick.OnPointerUp(eventData);
            }
            knobButton.OnPointerUp(eventData);
        }
    }

    private Vector3 ClampJoystickPositionToScreen(PointerEventData eventData)
    {
        Rect backgroundKnob = BackgroundCanvasGroup.GetComponent<RectTransform>().rect;

        if (eventData.position.x > transform.position.x - backgroundKnob.width / 2 * scaleCanvas)
        {
            positionX = transform.position.x - backgroundKnob.width / 2 * scaleCanvas;
        }
        else
        {
            positionX = eventData.position.x;
        }
        if (eventData.position.y < transform.position.y + backgroundKnob.height / 2 * scaleCanvas)
        {
            positionY = transform.position.y + backgroundKnob.height / 2 * scaleCanvas;
        }
        else
        {
            positionY = eventData.position.y;
        }

        _newPosition = new Vector3(positionX, positionY, 0f);
        return _newPosition;
    }
}
