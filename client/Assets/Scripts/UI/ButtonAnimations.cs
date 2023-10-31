using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class ButtonAnimations : MMTouchButton
{
    [SerializeField]
    bool isBackButton;

    public override void OnPointerDown(PointerEventData data)
    {
        base.OnPointerDown(data);
        transform.DOScale(new Vector3(0.965f, 0.965f, 0.965f), 0.5f);
    }

    public override void OnPointerUp(PointerEventData data)
    {
        base.OnPointerUp(data);
        transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f);
        if (isBackButton)
        {
            DOTween.Kill(transform);
        }
    }
}
