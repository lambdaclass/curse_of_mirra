using MoreMountains.Tools;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using System;

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
    private const float MIN_DIFFERENCE = 6.0f;
    private Vector2 touchStartPos;
    private bool isInsideCard = false;
    public bool executeRelease = false;

    void Start()
    {
        initialScale = transform.localScale;
        finalScale = initialScale - new Vector3(0.05f, 0.05f, 0.05f);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
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

        touchStartPos = eventData.position;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        if (isBackButton)
        {
            transform.DOPause();
        }
        else
        {
            transform.DOScale(initialScale, duration);
        }

        CheckReleasePosition(eventData);
    }

    public void CheckReleasePosition(PointerEventData eventData)
    {
        var touchXDifference = Math.Abs(eventData.position.x - touchStartPos.x);
        var touchYDifference = Math.Abs(eventData.position.y - touchStartPos.y);
        if (isInsideCard && touchXDifference < MIN_DIFFERENCE && touchYDifference < MIN_DIFFERENCE)
        {
            executeRelease = true;
        }
        else
        {
            executeRelease = false;
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        isInsideCard = false;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        isInsideCard = true;
    }
}
