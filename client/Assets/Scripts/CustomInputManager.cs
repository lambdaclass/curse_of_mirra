using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using MoreMountains.TopDownEngine;
using System;

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
    [SerializeField] GameObject mainAttack;
    [SerializeField] GameObject specialAttack;
    [SerializeField] GameObject dash;
    [SerializeField] GameObject ultimate;
    GameObject areaWithAim;
    GameObject area;
    GameObject indicator;

    public void AssignAbilityToInput(UIControls trigger, UIType triggerType, Ability ability)
    {
        // TODO: REFACTOR! The input type is hardcoded for each specific attack and shouw be configurable.

        if (trigger == UIControls.AttackBasic){
            UnityEvent<Ability> tapEvent = new UnityEvent<Ability>();
            tapEvent.AddListener(TapAttack);
            ultimate.GetComponent<CustomMMTouchJoystick>().newPointerTapEvent = tapEvent;
            ultimate.GetComponent<CustomMMTouchJoystick>().ability = ability;
        }

        if (trigger == UIControls.Attack2){
            UnityEvent aoeEvent = new UnityEvent();
            aoeEvent.AddListener(ShowAimDirectionAttack);
            ultimate.GetComponent<CustomMMTouchJoystick>().newPointerDownEvent = aoeEvent;

            UnityEvent<Vector2> aoeDragEvent = new UnityEvent<Vector2>();
            aoeDragEvent.AddListener(AimDirectionAttack);
            ultimate.GetComponent<CustomMMTouchJoystick>().newDragEvent = aoeDragEvent;

            UnityEvent<Vector2,Ability> aoeRelease = new UnityEvent<Vector2,Ability>();
            aoeRelease.AddListener(ExecuteAoeAttack);
            ultimate.GetComponent<CustomMMTouchJoystick>().ability = ability;
            ultimate.GetComponent<CustomMMTouchJoystick>().newPointerUpEvent = aoeRelease;
        }

        if (trigger == UIControls.Attack3){
            UnityEvent aoeEvent = new UnityEvent();
            aoeEvent.AddListener(ShowAimAoeAttack);
            ultimate.GetComponent<CustomMMTouchJoystick>().newPointerDownEvent = aoeEvent;

            UnityEvent<Vector2> aoeDragEvent = new UnityEvent<Vector2>();
            aoeDragEvent.AddListener(AimAoeAttack);
            ultimate.GetComponent<CustomMMTouchJoystick>().newDragEvent = aoeDragEvent;

            UnityEvent<Vector2,Ability> aoeRelease = new UnityEvent<Vector2,Ability>();
            aoeRelease.AddListener(ExecuteAoeAttack);
            ultimate.GetComponent<CustomMMTouchJoystick>().ability = ability;
            ultimate.GetComponent<CustomMMTouchJoystick>().newPointerUpEvent = aoeRelease;
        }
    }

  private void AimDirectionAttack(Vector2 arg0)
  {
    throw new NotImplementedException();
  }

  private void ShowAimDirectionAttack()
  {
    throw new NotImplementedException();
  }

  public void TapAttack(Ability ability){
        ability.ExecuteAbility();
    }

    public void ShowAimAoeAttack()
    {
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

        // FIXME: remove hardcoded attr
        SetJoystick(ultimate);
    }

    public void AimAoeAttack(Vector2 aoePosition)
    {
        GameObject _player = GameObject.Find("Player 1");

        //Multiply vector values according to the scale of the animation (in this case 12)
        indicator.transform.position = _player.transform.position + new Vector3(aoePosition.x * 12, 0f, aoePosition.y * 12);
    }
    public void ExecuteAoeAttack(Vector2 aoePosition, Ability ability)
    {
        GameObject _player = GameObject.Find("Player 1");

        //Destroy attack animation after showing it
        Destroy(areaWithAim, 2.1f);

        indicator.transform.position = _player.transform.position + new Vector3(aoePosition.x * 12, 0f, aoePosition.y * 12);
        Destroy(indicator, 0.01f);
        Destroy(area, 0.01f);
        // FIXME: remove hardcoded attr
        UnSetJoystick(ultimate);

        ability.ExecuteAbility(aoePosition);
    }


    public void SetJoystick(GameObject button)
    {
        Image joystickBg = button.transform.parent.gameObject.GetComponent<Image>();
        joystickBg.enabled = true;
    }
    public void UnSetJoystick(GameObject button)
    {
        Image joystickBg = button.transform.parent.gameObject.GetComponent<Image>();
        joystickBg.enabled = false;
    }
}
