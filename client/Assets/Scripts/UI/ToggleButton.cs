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

    [SerializeField]
    CustomLogs customLogs;

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

    public void ToggleClientPredictionGhost()
    {
        if (playerMovement.showClientPredictionGhost)
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

    public void ToggleInterpolationGhosts()
    {
        if (playerMovement.showInterpolationGhosts)
        {
            ToggleOn();
        }
        else
        {
            ToggleOff();
        }
    }

    public void ToggleAllLogs()
    {
        if (customLogs.debugPrint)
        {
            ToggleOn();
        }
        else
        {
            ToggleOff();
        }
    }

    public void ToggleCustomLogs()
    {
        if (CustomLogs.allowCustomDebug)
        {
            ToggleOn();
        }
        else
        {
            ToggleOff();
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
