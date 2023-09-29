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

    private static readonly Color white = new Color(1f, 1f, 1f);
    private static readonly Color grey = new Color(0.5f, 0.5f, 0.5f);

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
                image.color = white;
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
                image.color = grey;
            }
        }
    }
}
