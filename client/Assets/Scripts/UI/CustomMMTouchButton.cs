using UnityEngine;
using MoreMountains.Tools;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class CustomMMTouchButton : MMTouchButton
{
    public UnityEvent<Skill> newPointerTapUp;
    public UnityEvent<Skill> newPointerTapDown;

    public Skill skill;
    List<Image> images = new List<Image>();

    public void SetInitialSprite(Sprite newSprite, Sprite backgroundSprite)
    {
        GameObject parent = gameObject.transform.parent.gameObject;
        parent.SetActive(true);
        this._initialSprite = newSprite;
        this.DisabledSprite = newSprite;
        this.PressedSprite = newSprite;
        if (backgroundSprite)
        {
            Image bg = parent.GetComponent<Image>();
            bg.sprite = backgroundSprite;
        }
        images.AddRange(GetComponentsInChildren<Image>());
    }

    public override void OnPointerUp(PointerEventData data)
    {
        newPointerTapUp.Invoke(skill);
        base.OnPointerUp(data);
        foreach (Image image in images)
        {
            if (image.GetComponent<CustomMMTouchButton>() == null)
            {
                image.color = new Color(255f, 255f, 255f);
            }
        }
    }

    public override void OnPointerPressed()
    {
        newPointerTapDown.Invoke(skill);
        base.OnPointerPressed();
        foreach (Image image in images)
        {
            if (image.GetComponent<CustomMMTouchButton>() == null)
            {
                image.color = new Color(0.5f, 0.5f, 0.5f);
            }
        }
    }
}
