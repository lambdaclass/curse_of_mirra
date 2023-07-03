using UnityEngine;
using MoreMountains.Tools;

public class CustomMMTouchButton : MMTouchButton
{
    public void SetInitialSprite(Sprite newSprite)
    {
        this._initialSprite = newSprite;
        this.DisabledSprite = newSprite;
        this.PressedSprite = newSprite;
    }
}
