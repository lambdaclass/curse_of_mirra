using System;
using System.Collections;
using Cinemachine;
using DG.Tweening;
using MoreMountains.TopDownEngine;
using UnityEngine;

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

    Vector3 playerPosition = new Vector3(0, 0, 0);
    Vector3 cameraGoToPosition = new Vector3(0, 25, -15);

    void Start()
    {
        cameraFramingTransposer =
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        StartCoroutine(CameraCinematic());
    }

    IEnumerator CameraCinematic()
    {
        cinemachineVirtualCamera.transform.DOMove(cameraGoToPosition + playerPosition, 3);
        yield return new WaitForSeconds(3f);
        cinemachineVirtualCamera.ForceCameraPosition(
            cameraGoToPosition + playerPosition,
            cinemachineVirtualCamera.transform.rotation
        );
        yield return new WaitUntil(() => SocketConnectionManager.Instance.players.Count > 0);
        SetCameraToPlayer(SocketConnectionManager.Instance.playerId);
        GeneratePlayersList();
        prepareBattleContainer.GetComponent<CanvasGroup>().alpha = 0;
        playersContainer.GetComponent<CanvasGroup>().alpha = 1;
        yield return new WaitForSeconds(5f);
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

    private void SetCameraToPlayer(ulong playerID)
    {
        foreach (CustomCharacter player in CustomLevelManager.Instance.PlayerPrefabs)
        {
            if (UInt64.Parse(player.PlayerID) == playerID)
            {
                cinemachineVirtualCamera
                    .GetComponent<CinemachineCameraController>()
                    .SetTarget(player);
                cinemachineVirtualCamera
                    .GetComponent<CinemachineCameraController>()
                    .StartFollowing();
            }
        }
    }
}
