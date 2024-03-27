using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI textState;

    [SerializeField]
    Battle battle;

    [SerializeField]
    CustomLogs customLogs;

    // This is a quick fix until a refactor on this is done
    [Tooltip("Check this if the toogle is the client prediction toggle")]
    public bool clientPrediction;

    // This is a quick fix until a refactor on this is done
    [Tooltip("Check this if the toogle is the console toggle")]
    public bool console;

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
        if (customLogs != null)
        {
            if (console)
            {
                ToggleAllLogs();
            }
            else
            {
                ToggleCustomLogs();
            }
        }
        if (battle != null)
        {
            if (clientPrediction)
            {
                ToggleClientPrediction();
            }
            else
            {
                ToggleClientPredictionGhost();
                ToggleInterpolationGhosts();
                SetGridSettings();
            }
        }
    }

    public void ToggleClientPrediction()
    {
        if (battle.useClientPrediction)
        {
            if (textState != null)
            {
                textState.text = "On";
            }
        }
        else
        {
            if (textState != null)
            {
                textState.text = "Off";
            }
        }
        ToggleUIState(battle.useClientPrediction);
    }

    public void ToggleClientPredictionGhost()
    {
        ToggleUIState(battle.showClientPredictionGhost);
    }

    public void ToggleInterpolationGhosts()
    {
        ToggleUIState(battle.showInterpolationGhosts);
    }

    public void ToggleAllLogs()
    {
        ToggleUIState(customLogs.debugPrint);
    }

    public void ToggleCustomLogs()
    {
        ToggleUIState(CustomLogs.allowCustomDebug);
    }

    public void ToggleCamera(bool value)
    {
        ToggleUIState(!value);
    }

    public void ToggleMetrics(GameObject metricsComponent)
    {
        ToggleUIState(!metricsComponent.activeSelf);
        metricsComponent.SetActive(!metricsComponent.activeSelf);
    }

    public void ToggleGrid()
    {
        bool gridState = !battle.GetMapGrid().activeSelf;
        ToggleUIState(gridState);
        battle.GetMapGrid().SetActive(gridState);
    }

    public void SetGridSettings()
    {
        ToggleUIState(battle.GetMapGrid().activeSelf);
    }

    public void ToggleWithSiblingComponentBool(bool value)
    {
        ToggleUIState(value);
    }

    private void ToggleUIState(bool isOn)
    {
        GetComponent<Image>().enabled = isOn;
        int alpha = isOn ? 255 : 100;
        textState.color = new Color32(255, 255, 255, (byte)alpha);
    }
}
