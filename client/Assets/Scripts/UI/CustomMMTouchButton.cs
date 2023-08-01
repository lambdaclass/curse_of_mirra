using UnityEngine;
using MoreMountains.Tools;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CustomMMTouchButton : MMTouchButton
{
    public UnityEvent<Skill> newPointerTapUp;

    public Skill skill;

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

    public override void OnPointerUp(PointerEventData data)
    {
        newPointerTapUp.Invoke(skill);
        base.OnPointerUp(data);
    }
}
