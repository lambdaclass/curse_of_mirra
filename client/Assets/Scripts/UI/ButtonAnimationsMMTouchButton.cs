using MoreMountains.Tools;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class ButtonAnimationsMMTouchButton : MMTouchButton
{
    [SerializeField]
    bool isBackButton;

    [Header("Animation duration")]
    float duration = 0.25f;

    [Header("Animation Scale values")]
    [Tooltip(
        "Initial scale of the object to animate. If is not specified it will use the scale values of the rectTransform"
    )]
    [SerializeField]
    Vector3 initialScale = Vector3.zero;

    [Tooltip(
        "Final scale of the object to animate. If is not specified it will use the initial scale minus 0.1f"
    )]
    [SerializeField]
    Vector3 finalScale;

    void Start()
    {
        if (initialScale == Vector3.zero)
            initialScale = GetComponent<RectTransform>().localScale;
        if (finalScale == Vector3.zero)
        {
            finalScale = new Vector3(
                initialScale.x - 0.1f,
                initialScale.y - 0.1f,
                initialScale.z - 0.1f
            );
        }
    }

    public override void OnPointerDown(PointerEventData data)
    {
        base.OnPointerDown(data);
        if (isBackButton)
        {
            transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), duration).SetEase(Ease.OutQuad);
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
