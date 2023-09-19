using UnityEngine;
using MoreMountains.Tools;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CustomMMTouchButton : MMTouchButton
{
    public UnityEvent<Skill> newPointerTapUp;
    public UnityEvent<Skill> newPointerTapDown;

    public Skill skill;

    public void SetInitialSprite(Sprite newSprite, Sprite backgroundSprite)
    {
        GameObject parent = gameObject.transform.parent.gameObject;
        parent.SetActive(true);
        Debug.Log(newSprite.name);
        /* this._initialSprite = newSprite;
        this.DisabledSprite = newSprite;
        this.PressedSprite = newSprite; */
        GetComponentInChildren<Image>().sprite = newSprite;
        if (backgroundSprite)
        {
            Image bg = parent.GetComponent<Image>();
            bg.sprite = backgroundSprite;
        }
    }

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
