using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CustomInputManager : MonoBehaviour
{
    [SerializeField] GameObject mainAttack;
    [SerializeField] GameObject specialAttack;
    [SerializeField] GameObject dash;
    [SerializeField] GameObject ultimate;
    public Camera UiCamera;
    public void AssignInputToAbilityPosition(string trigger, string triggerType, UnityEvent abilityEvent)
    {
        if (triggerType == "joystick") & (trigger == "y")
        {
            specialAttack.GetComponent<CustomMMTouchJoystick>().newPointerDownEvent = abilityEvent;
            abilityEvent.AddListener(UiCamera.GetComponent<CustomInputManager>().SetJoystickAOE);
        }

        else if (triggerType == "joystick") & (trigger == "ultimate")
        {
            specialAttack.GetComponent<CustomMMTouchJoystick>().newPointerDownEvent = abilityEvent;
            ultimate.AddListener(UiCamera.GetComponent<CustomInputManager>().SetJoystickAOE);
        }
    }
    public void AssignInputToAimPosition(string trigger, string triggerType, UnityEvent<Vector2> aim)
    {
        if (triggerType == "joystick") & (trigger == "y")
        {
            specialAttack.GetComponent<CustomMMTouchJoystick>().newDragEvent = aim;
        }

        else if (triggerType == "joystick") & (trigger == "ultimate")
        {
            ultimate.GetComponent<CustomMMTouchJoystick>().newDragEvent = aim;
        }
    }
    public void AssignInputToAbilityExecution(string trigger, string triggerType, UnityEvent<Vector2> abilityPosition)
    {
        if (triggerType == "joystick") & (trigger == "y")
        {
            specialAttack.GetComponent<CustomMMTouchJoystick>().newPointerUpEvent = abilityPosition;
            abilityPosition.AddListener(UiCamera.GetComponent<CustomInputManager>().UnSetJoystick);
        }

        else if (triggerType == "joystick") & (trigger == "ultimate")
        {
            ultimate.GetComponent<CustomMMTouchJoystick>().newPointerUpEvent = abilityPosition;
            abilityPosition.AddListener(UiCamera.GetComponent<CustomInputManager>().UnSetJoystick);
        }
    }
    public void SetJoystickAOE()
    {
        Image joystickBg = specialAttack.transform.parent.gameObject.GetComponent<Image>();
        joystickBg.enabled = true;
    }
    public void UnSetJoystickAOE(Vector2 position)
    {
        Image joystickBg = specialAttack.transform.parent.gameObject.GetComponent<Image>();
        joystickBg.enabled = false;
    }

    public void SetJoystickUltimate()
    {
        Image joystickBg = ultimate.transform.parent.gameObject.GetComponent<Image>();
        joystickBg.enabled = true;
    }
    public void UnSetJoystickUltimate(Vector2 position)
    {
        Image joystickBg = ultimate.transform.parent.gameObject.GetComponent<Image>();
        joystickBg.enabled = false;
    }
}
