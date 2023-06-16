using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Events;
using MoreMountains.TopDownEngine;
using System;
using System.Collections.Generic;

public enum UIControls
{
    Skill1,
    Skill2,
    Skill3,
    Skill4,
    SkillBasic
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
    [SerializeField] GameObject SkillBasic;
    [SerializeField] GameObject Skill1;
    [SerializeField] GameObject Skill2;
    [SerializeField] GameObject Skill3;
    [SerializeField] GameObject Skill4;
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
        mobileButtons.Add(UIControls.Skill1, Skill1);
        mobileButtons.Add(UIControls.Skill2, Skill2);
        mobileButtons.Add(UIControls.Skill3, Skill3);
        // mobileButtons.Add(UIControls.Skill4, Skill4);
        mobileButtons.Add(UIControls.SkillBasic, SkillBasic);
    }

    public void AssignSkillToInput(UIControls trigger, UIType triggerType, Skill skill)
    {
        CustomMMTouchJoystick joystick = mobileButtons[trigger].GetComponent<CustomMMTouchJoystick>();

        switch (triggerType)
        {
            case UIType.Tap:
                MMTouchButton button = mobileButtons[trigger].GetComponent<MMTouchButton>();

                button.ButtonPressedFirstTime.AddListener(skill.ExecuteSkill);
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
                MapAreaInputEvents(joystick, skill);
                break;

            case UIType.Direction:
                if (joystick)
                {
                    joystick.enabled = true;
                }
                MapDirectionInputEvents(joystick, skill);
                break;
        }
    }

    private void MapAreaInputEvents(CustomMMTouchJoystick joystick, Skill skill)
    {
        UnityEvent<CustomMMTouchJoystick> aoeEvent = new UnityEvent<CustomMMTouchJoystick>();
        aoeEvent.AddListener(ShowAimAoeSkill);
        joystick.newPointerDownEvent = aoeEvent;

        UnityEvent<Vector2> aoeDragEvent = new UnityEvent<Vector2>();
        aoeDragEvent.AddListener(AimAoeSkill);
        joystick.newDragEvent = aoeDragEvent;

        UnityEvent<Vector2, Skill> aoeRelease = new UnityEvent<Vector2, Skill>();
        aoeRelease.AddListener(ExecuteAoeSkill);
        joystick.skill = skill;
        joystick.newPointerUpEvent = aoeRelease;
    }

    public void ShowAimAoeSkill(CustomMMTouchJoystick joystick)
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

    public void AimAoeSkill(Vector2 aoePosition)
    {
        // FIXME: Remove harcoded reference
        GameObject _player = GameObject.Find("Player 1");

        //Multiply vector values according to the scale of the animation (in this case 12)
        indicator.transform.position = _player.transform.position + new Vector3(aoePosition.x * 12, 0f, aoePosition.y * 12);
    }

    public void ExecuteAoeSkill(Vector2 aoePosition, Skill skill)
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

        skill.ExecuteSkill(aoePosition);
    }

    private void MapDirectionInputEvents(CustomMMTouchJoystick joystick, Skill skill)
    {
        UnityEvent<CustomMMTouchJoystick> directionEvent = new UnityEvent<CustomMMTouchJoystick>();
        directionEvent.AddListener(ShowAimDirectionSkill);
        joystick.newPointerDownEvent = directionEvent;

        UnityEvent<Vector2> directionDragEvent = new UnityEvent<Vector2>();
        directionDragEvent.AddListener(AimDirectionSkill);
        joystick.newDragEvent = directionDragEvent;

        UnityEvent<Vector2, Skill> directionRelease = new UnityEvent<Vector2, Skill>();
        directionRelease.AddListener(ExecuteDirectionSkill);
        joystick.skill = skill;
        joystick.newPointerUpEvent = directionRelease;
    }

    private void ShowAimDirectionSkill(CustomMMTouchJoystick joystick)
    {
        // FIXME: Remove harcoded reference
        GameObject _player = GameObject.Find("Player 1");

        areaWithAim = Instantiate(Resources.Load("AreaAim", typeof(GameObject))) as GameObject;
        //Set the prefav as a player child
        areaWithAim.transform.parent = _player.transform;
        //Set its position to the player position
        areaWithAim.transform.position = _player.transform.position;

        //Set scales
        area = areaWithAim.GetComponent<AimHandler>().area;
        area.transform.localScale = area.transform.localScale * 30;

        //Load the prefab
        directionIndicator = Instantiate(Resources.Load("AttackDirection", typeof(GameObject))) as GameObject;
        //Set the prefav as a player child
        directionIndicator.transform.parent = _player.transform;
        //Set its position to the player position
        directionIndicator.transform.position = new Vector3(_player.transform.position.x, 0.4f, _player.transform.position.z);

        // FIXME: Using harcoded value for testing, Value should be set dinamically
        directionIndicator.transform.localScale = new Vector3(directionIndicator.transform.localScale.x, area.transform.localScale.y * 2.45f, directionIndicator.transform.localScale.z);
        directionIndicator.SetActive(false);

        activeJoystick = joystick;
        DisableButtons();
    }

    private void AimDirectionSkill(Vector2 direction)
    {
        var result = Mathf.Atan(direction.x / direction.y) * Mathf.Rad2Deg;
        if (direction.y > 0)
        {
            result += 180f;
        }
        directionIndicator.transform.rotation = Quaternion.Euler(90f, result, 0);
        directionIndicator.SetActive(true);
    }

    private void ExecuteDirectionSkill(Vector2 direction, Skill skill)
    {
        Destroy(areaWithAim);
        Destroy(directionIndicator);

        activeJoystick = null;
        EnableButton();

        skill.ExecuteSkill(direction);
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
