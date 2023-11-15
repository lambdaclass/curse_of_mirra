using MoreMountains.Tools;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class ButtonAnimationsMMTouchButton : MMTouchButton
{
    [SerializeField]
    bool isBackButton;
    float duration = 0.25f;

    public override void OnPointerDown(PointerEventData data)
    {
        base.OnPointerDown(data);
        if (isBackButton)
        {
            transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), duration).SetEase(Ease.OutQuad);
        }
        else
        {
            transform.DOScale(new Vector3(0.965f, 0.965f, 0.965f), duration).SetEase(Ease.OutQuad);
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
            transform.DOScale(new Vector3(1f, 1f, 1f), duration);
        }
    }
}
