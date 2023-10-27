using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class ButtonAnimations : MMTouchButton
{
    public override void OnPointerDown(PointerEventData data)
    {
        base.OnPointerDown(data);
        transform.DOScale(new Vector3(0.96f, 0.96f, 0.96f), 0.5f);
    }

    public override void OnPointerUp(PointerEventData data)
    {
        base.OnPointerDown(data);
        transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f);
    }
}
