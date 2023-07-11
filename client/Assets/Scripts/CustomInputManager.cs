using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using MoreMountains.TopDownEngine;
using System;
using System.Collections.Generic;
using TMPro;

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
    [SerializeField] Image joystickL;
    [SerializeField] CustomMMTouchButton SkillBasic;
    [SerializeField] CustomMMTouchButton Skill1;
    [SerializeField] CustomMMTouchButton Skill2;
    [SerializeField] CustomMMTouchButton Skill3;
    [SerializeField] CustomMMTouchButton Skill4;
    [SerializeField] TMP_Text SkillBasicCooldown;
    [SerializeField] TMP_Text Skill1Cooldown;
    [SerializeField] TMP_Text Skill2Cooldown;
    [SerializeField] TMP_Text Skill3Cooldown;
    [SerializeField] TMP_Text Skill4Cooldown;
    Dictionary<UIControls, CustomMMTouchButton> mobileButtons;
    Dictionary<UIControls, TMP_Text> buttonsCooldown;
    private GameObject areaWithAim;
    private GameObject area;
    private GameObject indicator;
    private GameObject directionIndicator;
    private CustomMMTouchJoystick activeJoystick;
    private Vector3 initialLeftJoystickPosition;

    protected override void Start()
    {
        base.Start();

        mobileButtons = new Dictionary<UIControls, CustomMMTouchButton>();
        mobileButtons.Add(UIControls.Skill1, Skill1);
        mobileButtons.Add(UIControls.Skill2, Skill2);
        mobileButtons.Add(UIControls.Skill3, Skill3);
        mobileButtons.Add(UIControls.Skill4, Skill4);
        mobileButtons.Add(UIControls.SkillBasic, SkillBasic);

        // TODO: this could be refactored implementing a button parent linking button and cooldown text
        // or extending CustomMMTouchButton and linking its cooldown text
        buttonsCooldown = new Dictionary<UIControls, TMP_Text>();
        buttonsCooldown.Add(UIControls.Skill1, Skill1Cooldown);
        buttonsCooldown.Add(UIControls.Skill2, Skill2Cooldown);
        buttonsCooldown.Add(UIControls.Skill3, Skill3Cooldown);
        buttonsCooldown.Add(UIControls.Skill4, Skill4Cooldown);
        buttonsCooldown.Add(UIControls.SkillBasic, SkillBasicCooldown);
    }
    public void InitializeInputSprite(CoMCharacter characterInfo)
    {
        SkillBasic.SetInitialSprite(characterInfo.skillBasicSprite, null);
        Skill1.SetInitialSprite(characterInfo.skill1Sprite, characterInfo.skillBackground);
        Skill2.SetInitialSprite(characterInfo.skill2Sprite, characterInfo.skillBackground);
        Skill3.SetInitialSprite(characterInfo.skill3Sprite, characterInfo.skillBackground);
        Skill4.SetInitialSprite(characterInfo.skill4Sprite, characterInfo.skillBackground);
    }
    public void AssignSkillToInput(UIControls trigger, UIType triggerType, Skill skill)
    {
        CustomMMTouchJoystick joystick = mobileButtons[trigger].GetComponent<CustomMMTouchJoystick>();
        CustomMMTouchButton button = mobileButtons[trigger].GetComponent<CustomMMTouchButton>();

        switch (triggerType)
        {
            case UIType.Tap:
                button.ButtonPressedFirstTime.AddListener(skill.TryExecuteSkill);
                if (joystick)
                {
                    joystick.enabled = false;
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

        skill.TryExecuteSkill(aoePosition);
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

        skill.TryExecuteSkill(direction);
    }

    public void CheckSkillCooldown(UIControls control, ulong cooldown)
    {
        CustomMMTouchButton button = mobileButtons[control];
        TMP_Text cooldownText = buttonsCooldown[control];

        if (cooldown == 0)
        {
            button.EnableButton();
            cooldownText.gameObject.SetActive(false);
        }
        else
        {
            button.DisableButton();
            cooldownText.gameObject.SetActive(true);
            cooldownText.text = cooldown.ToString();
        }
    }

    private void DisableButtons()
    {
        foreach (var (key, button) in mobileButtons)
        {
            if (button != activeJoystick)
            {
                // Try CustomMMTouchButton.DisableButton();
                button.GetComponent<CustomMMTouchButton>().Interactable = false;
            }
        }
    }

    private void EnableButtons()
    {
        foreach (var (key, button) in mobileButtons)
        {
            button.GetComponent<CustomMMTouchButton>().Interactable = true;
        }
    }

    public void SetOpacity()
    {
        joystickL.color = new Color(255, 255, 255, 0.25f);
    }
    public void UnsetOpacity()
    {
        joystickL.color = new Color(255, 255, 255, 1);
    }
}
