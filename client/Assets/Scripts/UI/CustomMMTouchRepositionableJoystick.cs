using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomMMTouchRepositionableJoystick : MMTouchRepositionableJoystick
{
    float positionX;
    float positionY;
    float scaleCanvas;

    protected override void Start()
    {
        base.Start();
        scaleCanvas = GetComponentInParent<Canvas>().gameObject.transform.localScale.x;
        _initialPosition = BackgroundCanvasGroup.transform.position;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        ClampJoystickPositionToScreen(eventData);
        if (KnobCanvasGroup.GetComponent<CustomMMTouchJoystick>().isActiveAndEnabled)
        {
            BackgroundCanvasGroup.transform.position = _newPosition;
            KnobCanvasGroup.GetComponent<CustomMMTouchJoystick>().SetNeutralPosition(_newPosition);
            KnobCanvasGroup.GetComponent<CustomMMTouchJoystick>().OnPointerDown(eventData);
        }
        else
        {
            KnobCanvasGroup.GetComponent<CustomMMTouchButton>().OnPointerDown(eventData);
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        if (KnobCanvasGroup.GetComponent<CustomMMTouchJoystick>().isActiveAndEnabled)
        {
            KnobCanvasGroup.GetComponent<CustomMMTouchJoystick>().OnDrag(eventData);
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        if (ResetPositionToInitialOnRelease)
        {
            if (KnobCanvasGroup.GetComponent<CustomMMTouchJoystick>().isActiveAndEnabled)
            {
                BackgroundCanvasGroup.transform.position = _initialPosition;
                KnobCanvasGroup
                    .GetComponent<CustomMMTouchJoystick>()
                    .SetNeutralPosition(_initialPosition);
                KnobCanvasGroup.GetComponent<CustomMMTouchJoystick>().OnPointerUp(eventData);
            }
            else
            {
                KnobCanvasGroup.GetComponent<CustomMMTouchButton>().OnPointerUp(eventData);
            }
        }
    }

    private Vector3 ClampJoystickPositionToScreen(PointerEventData eventData)
    {
        Debug.Log("position: " + eventData.position);
        positionY = eventData.position.y;

        positionX = eventData.position.x;

        _newPosition = new Vector3(positionX, positionY, 0f);
        return _newPosition;
    }
}
