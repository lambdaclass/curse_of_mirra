using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using MoreMountains.Tools;

public class LeftMMTouchJoystick : MMTouchJoystick
{
    float adjustValue = 15f;
    float scaleCanvas;
    public override void Initialize(){
        base.Initialize();
    }
    public override void RefreshMaxRangeDistance()
    {
        // What makes this responsive is taking into account the canvas scaling
        scaleCanvas = GetComponentInParent<Canvas>().gameObject.transform.localScale.x;

        float knobBackgroundDiameter = gameObject.transform.parent.gameObject.GetComponent<RectTransform>().rect.width;
        float knobDiameter = GetComponent<RectTransform>().rect.width;
        MaxRange = (knobBackgroundDiameter - knobDiameter - adjustValue) * scaleCanvas;
        base.RefreshMaxRangeDistance();
    }
}
