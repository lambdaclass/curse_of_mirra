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
    Battle battle;

    [SerializeField]
    CustomLogs customLogs;

    void Start()
    {
        if (GetComponent<MMTouchButton>())
        {
            GetComponent<MMTouchButton>().ReturnToInitialSpriteAutomatically = false;
        }
        if (transform.parent.GetComponent<MMTouchButton>())
        {
            transform.parent.GetComponent<MMTouchButton>().ReturnToInitialSpriteAutomatically =
                false;
        }
    }

    public void ToggleClientPrediction()
    {
        if (battle.useClientPrediction)
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
        if (battle.showClientPredictionGhost)
        {
            ToggleOn();
        }
        else
        {
            ToggleOff();
        }
    }

    public void ToggleInterpolationGhosts()
    {
        if (battle.showInterpolationGhosts)
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

    public void ToggleCamera(bool value)
    {
        if (value)
        {
            ToggleOff();
        }
        else
        {
            ToggleOn();
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
