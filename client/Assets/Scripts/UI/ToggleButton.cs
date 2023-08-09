using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MoreMountains.Tools;

public class ToggleButton : MonoBehaviour
{
    [SerializeField]
    Sprite notSelectedButton;

    [SerializeField]
    Sprite selectedButton;

    [SerializeField]
    TextMeshProUGUI state;

    [SerializeField]
    PlayerMovement playerMovement;

    void Start()
    {
        GetComponent<MMTouchButton>().ReturnToInitialSpriteAutomatically = false;
    }

    public void ToggleClientPrediction()
    {
        if (playerMovement.useClientPrediction)
        {
            ToggleOn();
            state.text = "On";
        }
        else
        {
            ToggleOff();
            state.text = "Off";
        }
    }

    public void ToggleWithSiblingComponentBool(bool value)
    {
        if (value)
        {
            ToggleOn();
        }
        else
        {
            ToggleOff();
        }
    }

    public void ToggleOn()
    {
        GetComponent<Image>().sprite = selectedButton;
    }

    public void ToggleOff()
    {
        GetComponent<Image>().sprite = notSelectedButton;
    }
}
