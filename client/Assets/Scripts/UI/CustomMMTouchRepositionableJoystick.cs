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
        KnobCanvasGroup.GetComponent<CustomMMTouchButton>().OnPointerDown(eventData);
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
            KnobCanvasGroup.GetComponent<CustomMMTouchButton>().OnPointerUp(eventData);
        }
    }

    private Vector3 ClampJoystickPositionToScreen(PointerEventData eventData)
    {
        Debug.Log("data: " + eventData.position);
        Debug.Log("position: " + BackgroundCanvasGroup.GetComponent<RectTransform>().rect.width);
        if (
            eventData.position.x
            > transform.position.x - BackgroundCanvasGroup.GetComponent<RectTransform>().rect.width
        )
        {
            positionX =
                transform.position.x
                - BackgroundCanvasGroup.GetComponent<RectTransform>().rect.width;
        }
        else
        {
            positionX = eventData.position.x;
        }
        if (
            eventData.position.y
            < transform.position.y + BackgroundCanvasGroup.GetComponent<RectTransform>().rect.height
        )
        {
            positionY =
                transform.position.y
                + BackgroundCanvasGroup.GetComponent<RectTransform>().rect.height;
        }
        else
        {
            positionY = eventData.position.y;
        }

        _newPosition = new Vector3(positionX, positionY, 0f);
        return _newPosition;
    }
}
