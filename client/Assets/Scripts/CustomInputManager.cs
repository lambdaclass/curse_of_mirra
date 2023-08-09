using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using MoreMountains.TopDownEngine;
using System;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public enum UIControls
{
    Skill1,
    Skill2,
    Skill3,
    Skill4,
    SkillBasic
}

public enum UIIndicatorType
{
    Cone,
    Area,
    Arrow,
    None
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
    [SerializeField]
    Image joystickL;

    [SerializeField]
    CustomMMTouchButton SkillBasic;

    [SerializeField]
    CustomMMTouchButton Skill1;

    [SerializeField]
    CustomMMTouchButton Skill2;

    [SerializeField]
    CustomMMTouchButton Skill3;

    [SerializeField]
    CustomMMTouchButton Skill4;

    [SerializeField]
    TMP_Text SkillBasicCooldown;

    [SerializeField]
    TMP_Text Skill1Cooldown;

    [SerializeField]
    TMP_Text Skill2Cooldown;

    [SerializeField]
    TMP_Text Skill3Cooldown;

    [SerializeField]
    TMP_Text Skill4Cooldown;

    [SerializeField]
    GameObject disarmObjectSkill1;

    [SerializeField]
    GameObject disarmObjectSkill2;

    [SerializeField]
    GameObject disarmObjectSkill3;

    [SerializeField]
    GameObject disarmObjectSkill4;

    [SerializeField]
    GameObject cancelButton;

    [SerializeField]
    GameObject UIControlsWrapper;

    Dictionary<UIControls, CustomMMTouchButton> mobileButtons;
    Dictionary<UIControls, TMP_Text> buttonsCooldown;
    private GameObject areaWithAim;
    private GameObject area;
    private GameObject indicator;
    private GameObject directionIndicator;
    private CustomMMTouchJoystick activeJoystick;
    private Vector3 initialLeftJoystickPosition;
    private bool disarmed = false;

    private float currentSkillRadius = 0;
    private bool activeJoystickStatus = false;

    public bool canceled = false;

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

        UIControlsWrapper.GetComponent<CanvasGroup>().alpha = 0;
    }

    // void Update()
    // {
    //     activeJoystickStatus = activeJoystick != null ? true : false;
    //     cancelButton.SetActive(activeJoystickStatus);
    // }

    public void ActivateDisarmEffect(bool isDisarmed)
    {
        if (disarmed != isDisarmed)
        {
            if (isDisarmed)
            {
                DisableButtons();
                SkillBasic.GetComponent<CustomMMTouchButton>().Interactable = true;
            }
            else
            {
                EnableButtons();
            }
            disarmObjectSkill1.SetActive(isDisarmed);
            disarmObjectSkill2.SetActive(isDisarmed);
            disarmObjectSkill3.SetActive(isDisarmed);
            disarmObjectSkill4.SetActive(isDisarmed);
            disarmed = isDisarmed;
        }
    }

    public void InitializeInputSprite(CoMCharacter characterInfo)
    {
        SkillBasic.SetInitialSprite(characterInfo.skillBasicSprite, null);
        Skill1.SetInitialSprite(characterInfo.skill1Sprite, characterInfo.skillBackground);
        Skill2.SetInitialSprite(characterInfo.skill2Sprite, characterInfo.skillBackground);
        Skill3.SetInitialSprite(characterInfo.skill3Sprite, characterInfo.skillBackground);
        Skill4.SetInitialSprite(characterInfo.skill4Sprite, characterInfo.skillBackground);
    }

    public IEnumerator ShowInputs()
    {
        yield return new WaitForSeconds(.1f);

        UIControlsWrapper.GetComponent<CanvasGroup>().alpha = 1;
    }

    public void AssignSkillToInput(UIControls trigger, UIType triggerType, Skill skill)
    {
        CustomMMTouchJoystick joystick = mobileButtons[
            trigger
        ].GetComponent<CustomMMTouchJoystick>();
        CustomMMTouchButton button = mobileButtons[trigger].GetComponent<CustomMMTouchButton>();

        switch (triggerType)
        {
            case UIType.Tap:
                // button.ButtonReleased.AddListener(skill.TryExecuteSkill);
                if (joystick)
                {
                    joystick.enabled = false;
                }
                MapTapInputEvents(button, skill);
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

        UnityEvent<Vector2, CustomMMTouchJoystick> aoeDragEvent =
            new UnityEvent<Vector2, CustomMMTouchJoystick>();
        aoeDragEvent.AddListener(AimAoeSkill);
        joystick.newDragEvent = aoeDragEvent;

        UnityEvent<Vector2, Skill> aoeRelease = new UnityEvent<Vector2, Skill>();
        aoeRelease.AddListener(ExecuteAoeSkill);
        joystick.skill = skill;
        joystick.newPointerUpEvent = aoeRelease;
    }

    private void MapTapInputEvents(CustomMMTouchButton button, Skill skill)
    {
        UnityEvent<Skill> tapRelease = new UnityEvent<Skill>();
        tapRelease.AddListener(ExecuteTapSkill);
        button.skill = skill;
        button.newPointerTapUp = tapRelease;
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
        area.transform.localScale = area.transform.localScale * joystick.skill.GetSkillRadius();

        directionIndicator =
            Instantiate(Resources.Load("AttackDirection", typeof(GameObject))) as GameObject;
        //Set the prefab as a player child
        directionIndicator.transform.parent = _player.transform;

        directionIndicator.transform.localScale *= 5;

        // FIXME: Using harcoded value for testing, Value should be set dinamically
        //TODO : Add the spread area (amgle) depeding of the skill.json
        directionIndicator
            .GetComponent<SelectIndicator>()
            .ActivateIndicator(joystick.skill.GetIndicatorType());
        activeJoystick = joystick;
    }

    public void AimAoeSkill(Vector2 aoePosition, CustomMMTouchJoystick joystick)
    {
        GameObject _player = Utils.GetPlayer(SocketConnectionManager.Instance.playerId);

        //Multiply vector values according to the scale of the animation (in this case 12)
        float multiplier = joystick.skill.GetSkillRadius();
        directionIndicator.transform.position =
            _player.transform.position
            + new Vector3(aoePosition.x * multiplier, 0f, aoePosition.y * multiplier);
        activeJoystickStatus = canceled;
    }

    public void ExecuteAoeSkill(Vector2 aoePosition, Skill skill)
    {
        GameObject _player = Utils.GetPlayer(SocketConnectionManager.Instance.playerId);
        float multiplier = skill.GetSkillRadius();
        //Destroy attack animation after showing it
        Destroy(areaWithAim);

        directionIndicator.transform.position =
            _player.transform.position
            + new Vector3(aoePosition.x * multiplier, 0f, aoePosition.y * multiplier);
        Destroy(directionIndicator, 0.01f);
        directionIndicator.SetActive(false);
        activeJoystick = null;
        EnableButtons();

        if (!activeJoystickStatus)
        {
            skill.TryExecuteSkill(aoePosition);
        }
    }

    public void ExecuteTapSkill(Skill skill)
    {
        if (!canceled)
        {
            skill.TryExecuteSkill();
        }
    }

    private void MapDirectionInputEvents(CustomMMTouchJoystick joystick, Skill skill)
    {
        UnityEvent<CustomMMTouchJoystick> directionEvent = new UnityEvent<CustomMMTouchJoystick>();
        directionEvent.AddListener(ShowAimDirectionSkill);
        joystick.newPointerDownEvent = directionEvent;

        UnityEvent<Vector2, CustomMMTouchJoystick> directionDragEvent =
            new UnityEvent<Vector2, CustomMMTouchJoystick>();
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
        area.transform.localScale = area.transform.localScale * joystick.skill.GetSkillRadius();
        area.GetComponent<SpriteRenderer>().maskInteraction =
            SpriteMaskInteraction.VisibleOutsideMask;
        //Load the prefab
        directionIndicator =
            Instantiate(Resources.Load("AttackDirection", typeof(GameObject))) as GameObject;
        //Set the prefav as a player child
        directionIndicator.transform.parent = _player.transform;
        //Set its position to the player position
        directionIndicator.transform.position = new Vector3(
            _player.transform.position.x,
            0.4f,
            _player.transform.position.z
        );

        // FIXME: Using harcoded value for testing, Value should be set dinamically
        //TODO : Add the spread area (amgle) depeding of the skill.json
        directionIndicator
            .GetComponent<SelectIndicator>()
            .ActivateIndicator(joystick.skill.GetIndicatorType());

        directionIndicator.GetComponent<SelectIndicator>().viewDistance =
            joystick.skill.GetSkillRadius();

        directionIndicator.GetComponent<SelectIndicator>().fov = joystick.skill.GetIndicatorAngle();

        var cone = directionIndicator.GetComponent<SelectIndicator>().cone;

        float scaleX = directionIndicator.transform.localScale.x * joystick.skill.GetArroWidth();

        float scaleY = (3.45f * joystick.skill.GetSkillRadius()) / 12;

        if (joystick.skill.GetIndicatorType() == UIIndicatorType.Arrow)
        {
            directionIndicator.transform.localScale = new Vector3(
                scaleX,
                scaleY,
                directionIndicator.transform.localScale.z
            );
        }
        directionIndicator.SetActive(false);

        activeJoystick = joystick;
    }

    private void AimDirectionSkill(Vector2 direction, CustomMMTouchJoystick joystick)
    {
        bool isCone = joystick.skill.GetIndicatorType() == UIIndicatorType.Cone;
        var result = Mathf.Atan(direction.x / direction.y) * Mathf.Rad2Deg;
        if (direction.y > 0)
        {
            result += 180f;
        }
        directionIndicator.transform.rotation = Quaternion.Euler(
            90f,
            result,
            isCone ? -(180 - joystick.skill.GetIndicatorAngle()) / 2 : 0
        );
        directionIndicator.SetActive(true);
        activeJoystickStatus = canceled;
    }

    private void ExecuteDirectionSkill(Vector2 direction, Skill skill)
    {
        Destroy(areaWithAim);
        Destroy(directionIndicator);

        activeJoystick = null;
        EnableButtons();

        if (!activeJoystickStatus)
        {
            skill.TryExecuteSkill(direction);
        }
    }

    public void CheckSkillCooldown(UIControls control, float cooldown)
    {
        CustomMMTouchButton button = mobileButtons[control];
        TMP_Text cooldownText = buttonsCooldown[control];

        if ((cooldown < 1f && cooldown > 0f) || cooldown > 0f)
        {
            button.DisableButton();
            cooldownText.gameObject.SetActive(true);
            if (cooldown < 1f && cooldown > 0f)
            {
                cooldownText.text = String.Format("{0:0.0}", cooldown);
            }
            else
            {
                cooldownText.text = ((ulong)cooldown + 1).ToString();
            }
        }
        else
        {
            button.EnableButton();
            cooldownText.gameObject.SetActive(false);
        }
    }

    private void DisableButtons()
    {
        foreach (var (key, button) in mobileButtons)
        {
            if (button != activeJoystick)
            {
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

    public void SetCanceled(bool value)
    {
        canceled = value;
    }

    public void ToggleCanceled(bool value)
    {
        cancelButton.SetActive(value);
    }
}
