using System;
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

    [SerializeField]
    CinemachineVirtualCamera cinemachineVirtualCamera;
    CinemachineFramingTransposer cameraFramingTransposer;

    void Start()
    {
        cameraFramingTransposer =
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        StartCoroutine(CameraCinematic());
    }

    IEnumerator CameraCinematic()
    {
        yield return new WaitForSeconds(2f);
        yield return new WaitUntil(() => SocketConnectionManager.Instance.players.Count > 0);
        GeneratePlayersList();
        prepareBattleContainer.GetComponent<CanvasGroup>().alpha = 0;
        playersContainer.GetComponent<CanvasGroup>().alpha = 1;
        yield return new WaitForSeconds(5f);
        cameraFramingTransposer.m_TrackedObjectOffset = new Vector3(0, 0, 0);
        playersContainer.GetComponent<CanvasGroup>().alpha = 0;
        surviveContainer.GetComponent<CanvasGroup>().alpha = 1;
        yield return new WaitForSeconds(1f);
        surviveContainer.GetComponent<CanvasGroup>().alpha = 0;
        GetComponent<CanvasGroup>().alpha = 0;
        battleScreen.GetComponent<CanvasGroup>().alpha = 1;
        gameObject.SetActive(false);
    }

    void GeneratePlayersList()
    {
        SocketConnectionManager.Instance.players.ForEach(
            (player) =>
            {
                GameObject item = Instantiate(playerCard, playersTable.transform);
            }
        );
    }
}
