using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.VFX;

public class PrepareForBattleAnimations : MonoBehaviour
{
    [SerializeField]
    GameObject battleScreen,
        prepareBattleContainer,
        playersContainer,
        surviveContainer,
        playerCard,
        playersTable,
        ray;

    CinemachineFramingTransposer cameraFramingTransposer = null;

    void Start()
    {
        StartCoroutine(CameraCinematic());
    }

    IEnumerator CameraCinematic()
    {
        yield return new WaitForSeconds(1f);
        prepareBattleContainer.GetComponent<CanvasGroup>().alpha = 0;
        playersContainer.GetComponent<CanvasGroup>().alpha = 1;
        yield return new WaitForSeconds(1f);
        playersContainer.GetComponent<CanvasGroup>().alpha = 0;
        surviveContainer.GetComponent<CanvasGroup>().alpha = 1;
        yield return new WaitForSeconds(1f);
        surviveContainer.GetComponent<CanvasGroup>().alpha = 0;
        GetComponent<CanvasGroup>().alpha = 0;
        battleScreen.GetComponent<CanvasGroup>().alpha = 1;
    }

    void GeneratePlayersList() { }
}
