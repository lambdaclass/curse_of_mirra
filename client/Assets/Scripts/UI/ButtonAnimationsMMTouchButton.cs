using MoreMountains.Tools;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class ButtonAnimationsMMTouchButton : MMTouchButton
{
    [SerializeField]
    bool isBackButton;
    float duration = 0.2f;
    Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;
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
            transform
                .DOScale(initialScale - new Vector3(0.05f, 0.05f, 0.05f), duration)
                .SetEase(Ease.OutQuad);
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
