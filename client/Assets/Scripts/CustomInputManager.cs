using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
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
    [SerializeField] Font TestFont;
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
        GameObject _player = Utils.GetPlayer(SocketConnectionManager.Instance.playerId);
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
        GameObject _player = Utils.GetPlayer(SocketConnectionManager.Instance.playerId);

        //Multiply vector values according to the scale of the animation (in this case 12)
        indicator.transform.position = _player.transform.position + new Vector3(aoePosition.x * 12, 0f, aoePosition.y * 12);
    }

    public void ExecuteAoeSkill(Vector2 aoePosition, Skill skill)
    {
        GameObject _player = Utils.GetPlayer(SocketConnectionManager.Instance.playerId);

        //Destroy attack animation after showing it
        Destroy(areaWithAim, 2.1f);

        indicator.transform.position = _player.transform.position + new Vector3(aoePosition.x * 12, 0f, aoePosition.y * 12);
        Destroy(indicator, 0.01f);
        Destroy(area, 0.01f);

        activeJoystick = null;
        EnableButtons();

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
        GameObject _player = Utils.GetPlayer(SocketConnectionManager.Instance.playerId);

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
        EnableButtons();

        skill.ExecuteSkill(direction);
    }

    public void CheckSkillCooldown(UIControls control, ulong cooldown){
        MMTouchButton button = mobileButtons[control].GetComponent<MMTouchButton>();
        GameObject cooldownObj = GameObject.Find(control.ToString());

        if (cooldown == 0){
            if (cooldownObj){
                button.EnableButton();
                cooldownObj.SetActive(false);
            }
        } else {
            button.DisableButton();

            // FIXME: refactor assignations
            cooldownObj = GameObject.Find(control.ToString());
            Text coolDownText;

            if (!cooldownObj){
                cooldownObj = new GameObject(control.ToString());
                cooldownObj.AddComponent<Text>();
                coolDownText = cooldownObj.GetComponent<Text>();
                coolDownText.transform.SetParent(button.transform.parent, false);
                coolDownText.font = TestFont;
                coolDownText.alignment = TextAnchor.MiddleCenter;
                coolDownText.fontSize = 76;
            } else {
                cooldownObj.SetActive(true);
                coolDownText = cooldownObj.GetComponent<Text>();
            }

            coolDownText.text = cooldown.ToString();
        }
    }

    private void DisableButtons()
    {
        foreach (var (key, button) in mobileButtons)
        {
            if (button != activeJoystick)
            {
                // Try MMTouchButton.DisableButton();
                button.GetComponent<MMTouchButton>().Interactable = false;
            }
        }
    }

    private void EnableButtons()
    {
        foreach (var (key, button) in mobileButtons)
        {
            button.GetComponent<MMTouchButton>().Interactable = true;
        }
    }
}
