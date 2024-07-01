using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BattleSettingsController : MonoBehaviour
{
    [SerializeField]
    Battle battle;

    [SerializeField]
    CustomLogs customLogs;

    public ToggleButton animationsButton;
    public ToggleButton zoneButton;
    public ToggleButton botsButton;
    public ToggleButton clientPredictionGhostButton;
    public ToggleButton interpolationGhostButton;
    public ToggleButton gridButton;
    public ToggleButton metricsButton;
    public ToggleButton clientPredictionButton;
    public ToggleButton consoleLogsButton;
    public ToggleButton customLogsButton;
    public ToggleButton reconciliationButton;
    public Slider reconciliationSlider;
    public TextMeshProUGUI reconciliationText;

    void Start()
    {
        StartCoroutine(InitializeToggles());
    }

    IEnumerator InitializeToggles()
    {
        yield return new WaitUntil(() => battle != null);
        ToggleZone();
        ToggleBots();
        ToggleAnimations();
        ToggleClientPredictionGhost();
        ToggleInterpolationGhosts();
        ToggleGrid();
        ToggleClientPrediction();
        yield return new WaitUntil(() => customLogs != null);
        ToggleConsoleLogs();
        ToggleCustomLogs();
    }

    public void ToggleZone()
    {
        zoneButton.ToggleUIState(battle.zoneActive);
    }

    public void ToggleBots()
    {
        botsButton.ToggleUIState(battle.botsActive);
    }

    public void ToggleAnimations()
    {
        animationsButton.ToggleUIState(battle.animationsEnabled);
    }

    public void ToggleClientPredictionGhost()
    {
        clientPredictionGhostButton.ToggleUIState(battle.showClientPredictionGhost);
    }

    public void ToggleInterpolationGhosts()
    {
        interpolationGhostButton.ToggleUIState(battle.showInterpolationGhosts);
    }

    public void ToggleGrid()
    {
        gridButton.ToggleUIState(battle.GetMapGrid().activeSelf);
    }

    public void ToggleMetrics(GameObject metricsComponent)
    {
        bool metricsState = !metricsComponent.activeSelf;
        metricsButton.ToggleUIState(metricsState);
        metricsComponent.SetActive(metricsState);
    }

    public void ToggleClientPrediction()
    {
        bool useClientPrediction = battle.useClientPrediction;
        if (useClientPrediction)
        {

            clientPredictionButton.textState.text = "On";
        }
        else
        {
            clientPredictionButton.textState.text = "Off";
        }
        clientPredictionButton.ToggleUIState(useClientPrediction);
    }

    public void ToggleConsoleLogs()
    {
        consoleLogsButton.ToggleUIState(customLogs.debugPrint);
    }

    public void ToggleCustomLogs()
    {
        customLogsButton.ToggleUIState(CustomLogs.allowCustomDebug);
    }

    public void ToggleReconciliation()
    {
        reconciliationButton.ToggleUIState(battle.useReconciliation);
    }

    public void ChangeReconciliationText()
    {
        reconciliationText.text = reconciliationSlider.value.ToString();
    }

    public void ChangeReconciliationValue()
    {
        battle.reconciliationDistance = reconciliationSlider.value;
    }
}
