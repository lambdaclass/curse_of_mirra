using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    [SerializeField]
    public Sprite onSprite;

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
        GetComponent<Image>().sprite = onSprite;
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
            ToggleUIState(true);
            if (textState != null)
            {
                textState.text = "On";
            }
        }
        else
        {
            ToggleUIState(false);

            if (textState != null)
            {
                textState.text = "Off";
            }
        }
    }

    public void ToggleClientPredictionGhost()
    {
        if (battle.showClientPredictionGhost)
        {
            ToggleUIState(true);
        }
        else
        {
            ToggleUIState(false);
        }
    }

    public void ToggleInterpolationGhosts()
    {
        if (battle.showInterpolationGhosts)
        {
            ToggleUIState(true);
        }
        else
        {
            ToggleUIState(false);
        }
    }

    public void ToggleAllLogs()
    {
        if (customLogs.debugPrint)
        {
            ToggleUIState(true);
        }
        else
        {
            ToggleUIState(false);
        }
    }

    public void ToggleCustomLogs()
    {
        if (CustomLogs.allowCustomDebug)
        {
            ToggleUIState(true);
        }
        else
        {
            ToggleUIState(false);
        }
    }

    public void ToggleCamera(bool value)
    {
        if (value)
        {
            ToggleUIState(false);
        }
        else
        {
            ToggleUIState(true);
        }
    }

    public void ToggleMetrics(GameObject metricsComponent)
    {
        if (metricsComponent.activeSelf)
        {
            ToggleUIState(false);
            metricsComponent.SetActive(false);
        }
        else
        {
            ToggleUIState(true);
            metricsComponent.SetActive(true);
        }
    }

    public void ToggleGrid()
    {
        if (battle.GetMapGrid().activeSelf)
        {
            ToggleUIState(false);
            battle.GetMapGrid().SetActive(false);
        }
        else
        {
            ToggleUIState(true);
            battle.GetMapGrid().SetActive(true);
        }
    }

    public void SetGridSettings()
    {
        if (battle.GetMapGrid().activeSelf)
        {
            ToggleUIState(true);
        }
        else
        {
            ToggleUIState(false);
        }
    }

    public void ToggleWithSiblingComponentBool(bool value)
    {
        if (value)
        {
            ToggleUIState(true);
        }
        else
        {
            ToggleUIState(false);
        }
    }

    private void ToggleUIState(bool isOn)
    {
        GetComponent<Image>().enabled = isOn;
        int alpha = isOn ? 255 : 100;
        textState.color = new Color32(255, 255, 255, (byte)alpha);
    }
}
