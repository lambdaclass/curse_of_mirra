using System;
using DG.Tweening;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAnimationsMMTouchButton : MMTouchButton
{
    [Header("Back Button bool")]
    [Tooltip(
        "If this component is applied to a back button this should be true to avoid animation errors"
    )]
    [SerializeField]
    bool isBackButton;

    [Header("Animation Scale values")]
    [Tooltip(
        "Initial scale of the object to animate. If is not specified it will use the scale values of the rectTransform"
    )]
    Vector3 initialScale;

    [Header("Animation duration")]
    float duration = 0.25f;

    [Tooltip(
        "Final scale of the object to animate. If is not specified it will use the initial scale minus 0.1f"
    )]
    Vector3 finalScale;

    //Min difference of the touchStartPos and the current touch
    private const float MIN_DIFFERENCE = 10.0f;

    private Vector2 touchStartPos;
    private bool isInsideCard = false;
    public bool executeRelease = false;

    [Header("List Element Bool")]
    [Tooltip(
        "The listElement bool is true when applied to list elements. This adds a logic to distinguish between a scroll and a selection of the element"
    )]
    public bool listElement = false;
    public bool characterModel = false;

    void Start()
    {
        initialScale = transform.localScale;
        finalScale = initialScale - new Vector3(0.05f, 0.05f, 0.05f);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        if (!characterModel)
        {
            if (isBackButton)
            {
                transform
                    .DOScale(initialScale - new Vector3(0.1f, 0.1f, 0.1f), duration)
                    .SetEase(Ease.OutQuad);
            }
            else
            {
                transform.DOScale(finalScale, duration).SetEase(Ease.OutQuad);
            }
        }

        touchStartPos = eventData.position;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (CheckReleasePosition(eventData))
        {
            base.OnPointerUp(eventData);
        }
        else
        {
            CurrentState = ButtonStates.Off;
        }
        if (!characterModel)
        {
            if (isBackButton)
            {
                transform.DOPause();
            }
            else
            {
                transform.DOScale(initialScale, duration);
            }
        }
    }

    public bool CheckReleasePosition(PointerEventData eventData)
    {
        // Issue #1542
        // This should check if the release of the pointer was inside or outside the container
        // not by position difference
        var touchXDifference = Math.Abs(eventData.position.x - touchStartPos.x);
        var touchYDifference = Math.Abs(eventData.position.y - touchStartPos.y);
        if (isInsideCard && (characterModel || listElement))
        {
            executeRelease = touchXDifference < MIN_DIFFERENCE && touchYDifference < MIN_DIFFERENCE;
        }
        else
        {
            executeRelease = isInsideCard && !(characterModel || listElement);
        }
        return executeRelease;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        transform.DOScale(initialScale, duration).SetEase(Ease.OutQuad);
        isInsideCard = false;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        isInsideCard = true;
    }
}
