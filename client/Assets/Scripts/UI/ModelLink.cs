using System;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.EventSystems;

public class ModelLink : MMTouchButton, IDragHandler
{
    [SerializeField]
    RotateUIModel uiModelRotator;
    bool isInsideContainer;
    Vector3 touchStartPos;
    private const float MIN_DIFFERENCE = 10.0f;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        uiModelRotator.OnPointerDown(eventData);
        touchStartPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        uiModelRotator.OnDrag(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (PointerGoToAction(eventData))
        {
            base.OnPointerUp(eventData);
        }
        else
        {
            CurrentState = ButtonStates.Off;
        }
        uiModelRotator.OnPointerUp(eventData);
    }

    bool PointerGoToAction(PointerEventData eventData)
    {
        bool executeAction;
        var touchXDifference = Math.Abs(eventData.position.x - touchStartPos.x);
        var touchYDifference = Math.Abs(eventData.position.y - touchStartPos.y);
        if (isInsideContainer)
        {
            executeAction = touchXDifference < MIN_DIFFERENCE && touchYDifference < MIN_DIFFERENCE;
        }
        else
        {
            executeAction = isInsideContainer;
        }
        return executeAction;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        isInsideContainer = false;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        isInsideContainer = true;
    }
}
