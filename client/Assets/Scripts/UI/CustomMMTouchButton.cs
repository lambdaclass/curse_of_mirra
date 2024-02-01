using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class CustomMMTouchButton : MMTouchButton
{
    public UnityEvent<Skill> newPointerTapUp;
    public UnityEvent<Skill> newPointerTapDown;

    public Skill skill;

    public override void OnPointerUp(PointerEventData data)
    {
        newPointerTapUp.Invoke(skill);
        base.OnPointerUp(data);
    }

    public override void OnPointerPressed()
    {
        newPointerTapDown.Invoke(skill);
        base.OnPointerPressed();
    }
}
