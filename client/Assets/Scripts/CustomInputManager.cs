using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using MoreMountains.TopDownEngine;

public class CustomInputManager : InputManager
{
    [SerializeField] GameObject mainAttack;
    [SerializeField] GameObject specialAttack;
    [SerializeField] GameObject dash;
    [SerializeField] GameObject ultimate;
    public Camera UiCamera;

    public Vector2 customInputPosition;

    public void AssignInputToAbilityPosition(string trigger, string triggerType, UnityEvent abilityEvent, Weapon action)
    {
        if (triggerType == "joystick")
        {
            if (trigger == "special"){
                specialAttack.GetComponent<CustomMMTouchJoystick>().newPointerDownEvent = abilityEvent;
            }
            if (trigger == "ultimate"){
                ultimate.GetComponent<CustomMMTouchJoystick>().newPointerDownEvent = abilityEvent;
                ultimate.GetComponent<CustomMMTouchJoystick>().action = action;
            }
            abilityEvent.AddListener(UiCamera.GetComponent<CustomInputManager>().SetJoystick);
        }
    }
    public void AssignInputToAimPosition(string trigger, string triggerType, UnityEvent<Vector2> aim)
    {
        if (triggerType == "joystick")
        {
            if (trigger == "special"){
                specialAttack.GetComponent<CustomMMTouchJoystick>().newDragEvent = aim;
            }
            if (trigger == "ultimate"){
                ultimate.GetComponent<CustomMMTouchJoystick>().newDragEvent = aim;
            }
        }
    }
    public void AssignInputToAbilityExecution(string trigger, string triggerType, UnityEvent<Vector2, Weapon> abilityPosition)
    {
        if (triggerType == "joystick")
        {
            if (trigger == "special"){
                specialAttack.GetComponent<CustomMMTouchJoystick>().newPointerUpEvent = abilityPosition;
            }
            if (trigger == "ultimate"){
                ultimate.GetComponent<CustomMMTouchJoystick>().newPointerUpEvent = abilityPosition;
            }
            abilityPosition.AddListener(UiCamera.GetComponent<CustomInputManager>().UnSetJoystick);
        }
    }
    public void SetJoystick()
    {
        Image joystickBg = specialAttack.transform.parent.gameObject.GetComponent<Image>();
        joystickBg.enabled = true;
    }
    public void UnSetJoystick(Vector2 position, Weapon _)
    {
        Image joystickBg = specialAttack.transform.parent.gameObject.GetComponent<Image>();
        joystickBg.enabled = false;
    }
}
