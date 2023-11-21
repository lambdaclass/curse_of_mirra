using MoreMountains.Tools;
using UnityEngine;
using DG.Tweening;
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

    void Start()
    {
        initialScale = transform.localScale;
        finalScale = initialScale - new Vector3(0.05f, 0.05f, 0.05f);
    }

    public override void OnPointerDown(PointerEventData data)
    {
        base.OnPointerDown(data);
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

    public override void OnPointerUp(PointerEventData data)
    {
        base.OnPointerUp(data);
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
