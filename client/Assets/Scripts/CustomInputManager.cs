using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Events;
using MoreMountains.TopDownEngine;
using System;
using System.Collections.Generic;

public enum UIControls
{
    Attack1,
    Attack2,
    Attack3,
    Attack4,
    AttackBasic
}
public enum UIType
{
    Tap,
    Area,
    Direction,
    Select
}

public class CustomInputManager : InputManager
{
    [SerializeField] GameObject AttackBasic;
    [SerializeField] GameObject Attack1;
    [SerializeField] GameObject Attack2;
    [SerializeField] GameObject Attack3;
    [SerializeField] GameObject Attack4;
    Dictionary<UIControls, GameObject> mobileButtons;
    private GameObject areaWithAim;
    private GameObject area;
    private GameObject indicator;
    private GameObject directionIndicator;
    private CustomMMTouchJoystick activeJoystick;

    protected override void Start()
    {
        base.Start();

        mobileButtons = new Dictionary<UIControls, GameObject>();
        mobileButtons.Add(UIControls.Attack1, Attack1);
        mobileButtons.Add(UIControls.Attack2, Attack2);
        mobileButtons.Add(UIControls.Attack3, Attack3);
        // mobileButtons.Add(UIControls.Attack4, Attack4);
        mobileButtons.Add(UIControls.AttackBasic, AttackBasic);
    }

    public void AssignAbilityToInput(UIControls trigger, UIType triggerType, Ability ability)
    {
        CustomMMTouchJoystick joystick = mobileButtons[trigger].GetComponent<CustomMMTouchJoystick>();

        switch (triggerType)
        {
            case UIType.Tap:
                MMTouchButton button = mobileButtons[trigger].GetComponent<MMTouchButton>();

                button.ButtonReleased.AddListener(ability.ExecuteAbility);
                if (joystick)
                {
                    mobileButtons[trigger].GetComponent<CustomMMTouchJoystick>().enabled = false;
                }
                break;

            case UIType.Area:
                if (joystick)
                {
                    joystick.enabled = true;
                }
                MapAreaInputEvents(joystick, ability);
                break;

            case UIType.Direction:
                if (joystick)
                {
                    joystick.enabled = true;
                }
                MapDirectionInputEvents(joystick, ability);
                break;
        }
    }

    private void MapAreaInputEvents(CustomMMTouchJoystick joystick, Ability ability)
    {
        UnityEvent<CustomMMTouchJoystick> aoeEvent = new UnityEvent<CustomMMTouchJoystick>();
        aoeEvent.AddListener(ShowAimAoeAttack);
        joystick.newPointerDownEvent = aoeEvent;

        UnityEvent<Vector2> aoeDragEvent = new UnityEvent<Vector2>();
        aoeDragEvent.AddListener(AimAoeAttack);
        joystick.newDragEvent = aoeDragEvent;

        UnityEvent<Vector2, Ability> aoeRelease = new UnityEvent<Vector2, Ability>();
        aoeRelease.AddListener(ExecuteAoeAttack);
        joystick.ability = ability;
        joystick.newPointerUpEvent = aoeRelease;
    }

    public void ShowAimAoeAttack(CustomMMTouchJoystick joystick)
    {
        if (activeJoystick == null)
        {
            // FIXME: Remove harcoded reference
            GameObject _player = GameObject.Find("Player 1");
            //Load the prefab
            areaWithAim = Instantiate(Resources.Load("AreaAim", typeof(GameObject))) as GameObject;
            //Set the prefav as a player child
            areaWithAim.transform.parent = _player.transform;
            //Set its position to the player position
            areaWithAim.transform.position = _player.transform.position;

            //Set scales
            area = areaWithAim.GetComponent<AimHandler>().area;
            area.transform.localScale = area.transform.localScale * 30;
            indicator = areaWithAim.GetComponent<AimHandler>().indicator;
            indicator.transform.localScale = indicator.transform.localScale * 5;

            activeJoystick = joystick;
            DisableButtons();
        }
    }

    public void AimAoeAttack(Vector2 aoePosition)
    {
        // FIXME: Remove harcoded reference
        GameObject _player = GameObject.Find("Player 1");

        //Multiply vector values according to the scale of the animation (in this case 12)
        indicator.transform.position = _player.transform.position + new Vector3(aoePosition.x * 12, 0f, aoePosition.y * 12);
    }

    public void ExecuteAoeAttack(Vector2 aoePosition, Ability ability)
    {
        // FIXME: Remove harcoded reference
        GameObject _player = GameObject.Find("Player 1");

        //Destroy attack animation after showing it
        Destroy(areaWithAim, 2.1f);

        indicator.transform.position = _player.transform.position + new Vector3(aoePosition.x * 12, 0f, aoePosition.y * 12);
        Destroy(indicator, 0.01f);
        Destroy(area, 0.01f);
        activeJoystick = null;
        EnableButton();

        ability.ExecuteAbility(aoePosition);
    }

    private void MapDirectionInputEvents(CustomMMTouchJoystick joystick, Ability ability)
    {
        UnityEvent<CustomMMTouchJoystick> directionEvent = new UnityEvent<CustomMMTouchJoystick>();
        directionEvent.AddListener(ShowAimDirectionAttack);
        joystick.newPointerDownEvent = directionEvent;

        UnityEvent<Vector2> directionDragEvent = new UnityEvent<Vector2>();
        directionDragEvent.AddListener(AimDirectionAttack);
        joystick.newDragEvent = directionDragEvent;

        UnityEvent<Vector2, Ability> directionRelease = new UnityEvent<Vector2, Ability>();
        directionRelease.AddListener(ExecuteDirectionAttack);
        joystick.ability = ability;
        joystick.newPointerUpEvent = directionRelease;
    }

    private void ShowAimDirectionAttack(CustomMMTouchJoystick joystick)
    {
        // FIXME: Remove harcoded reference
        GameObject _player = GameObject.Find("Player 1");

        //Load the prefab
        directionIndicator = Instantiate(Resources.Load("AttackDirection", typeof(GameObject))) as GameObject;
        //Set the prefav as a player child
        directionIndicator.transform.parent = _player.transform;
        //Set its position to the player position
        directionIndicator.transform.position = new Vector3(_player.transform.position.x, 0.4f, _player.transform.position.z);
    }

    private void AimDirectionAttack(Vector2 direction)
    {
        var result = Mathf.Atan(direction.x / direction.y) * Mathf.Rad2Deg;
        if (direction.y > 0)
        {
            result += 180f;
        }
        directionIndicator.transform.rotation = Quaternion.Euler(90f, result, 0);
    }

    private void ExecuteDirectionAttack(Vector2 direction, Ability ability)
    {
        Destroy(directionIndicator);
    }

    private void DisableButtons()
    {
        foreach (var (key, button) in mobileButtons)
        {
            if (button != activeJoystick)
            {
                button.GetComponent<MMTouchButton>().Interactable = false;
            }
        }
    }

    private void EnableButton()
    {
        foreach (var (key, button) in mobileButtons)
        {
            button.GetComponent<MMTouchButton>().Interactable = true;
        }
    }
}
