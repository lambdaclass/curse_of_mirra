using UnityEngine;
using MoreMountains.Tools;

public class LeftMMTouchJoystick : MMTouchJoystick
{
    // Needed due to padding in the sprite
    float adjustValue = 30f;

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void RefreshMaxRangeDistance()
    {
        // What makes this responsive is taking into account the canvas scaling
        float scaleCanvas = GetComponentInParent<Canvas>().transform.localScale.x;

        float knobBackgroundRadius =
            gameObject.transform.parent.GetComponent<RectTransform>().rect.width / 2;
        float knobRadius = GetComponent<RectTransform>().rect.width / 2;
        MaxRange = (knobBackgroundRadius - knobRadius + adjustValue) * scaleCanvas;
        base.RefreshMaxRangeDistance();
    }
}
