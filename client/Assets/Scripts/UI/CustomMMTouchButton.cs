using UnityEngine;
using MoreMountains.Tools;
using UnityEngine.UI;

public class CustomMMTouchButton : MMTouchButton
{
    public void SetInitialSprite(Sprite newSprite, Sprite backgroundSprite)
    {
        GameObject parent1 = gameObject.transform.parent.gameObject;
        parent1.SetActive(true);
        this._initialSprite = newSprite;
        this.DisabledSprite = newSprite;
        this.PressedSprite = newSprite;
        if (backgroundSprite)
        {
            Image bg = parent1.GetComponent<Image>();
            bg.sprite = backgroundSprite;
        }
    }
}
