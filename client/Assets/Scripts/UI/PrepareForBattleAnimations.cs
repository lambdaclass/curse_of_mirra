using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using DG.Tweening;
using MoreMountains.TopDownEngine;
using TMPro;
using UnityEngine;

public class PrepareForBattleAnimations : MonoBehaviour
{
    [SerializeField]
    GameObject battleScreen,
        loadingScreen,
        prepareBattleContainer,
        playersContainer,
        surviveContainer,
        playerCard,
        playersTopTable,
        playersBottomTable,
        ray,
        prepareCoin;

    [SerializeField]
    TextMeshProUGUI countDown;

    [SerializeField]
    CinemachineVirtualCamera cinemachineVirtualCamera;
    CinemachineFramingTransposer cameraFramingTransposer;

    Vector3 cameraOffsetPosition = new Vector3(0, 25.7f, -15);

    const float TIME_UNTIL_GAME_STARTS = 5f;
    float originalCoinScale;

    void Start()
    {
        cameraFramingTransposer =
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        originalCoinScale = prepareCoin.transform.localScale.x;
        StartCoroutine(CameraCinematic());
    }

    IEnumerator CameraCinematic()
    {
        yield return new WaitUntil(() => SocketConnectionManager.Instance.players.Count > 0);
        loadingScreen.GetComponent<CanvasGroup>().DOFade(0, .5f);
        StartCoroutine(PrepareForBattleAnimation());
        yield return new WaitForSeconds(4f);
        StartCoroutine(PlayersAnimation());
        yield return new WaitForSeconds(5f);
        StartCoroutine(SurviveAnimation());
        yield return new WaitForSeconds(2f);
        GetComponent<CanvasGroup>().DOFade(0, .5f);
        battleScreen.GetComponent<CanvasGroup>().DOFade(1, .5f);
        gameObject.SetActive(false);
    }

    IEnumerator PrepareForBattleAnimation()
    {
        prepareBattleContainer.GetComponent<CanvasGroup>().DOFade(1, .5f);
        PulseAnimation(prepareCoin, originalCoinScale);
        GameObject player = Utils.GetPlayer(SocketConnectionManager.Instance.playerId);
        cinemachineVirtualCamera.transform.DOMove(
            player.transform.position + cameraOffsetPosition,
            4
        );
        yield return new WaitForSeconds(4f);
        cinemachineVirtualCamera.ForceCameraPosition(
            player.transform.position + cameraOffsetPosition,
            cinemachineVirtualCamera.transform.rotation
        );
        SetCameraToPlayer(SocketConnectionManager.Instance.playerId);
        prepareBattleContainer.GetComponent<CanvasGroup>().DOFade(0, .5f);
    }

    IEnumerator PlayersAnimation()
    {
        GeneratePlayersList();
        playersContainer.GetComponent<CanvasGroup>().DOFade(1, .5f);
        List<PlayerCardManager> cardsTopTable = playersTopTable
            .GetComponentsInChildren<PlayerCardManager>()
            .ToList();
        List<PlayerCardManager> cardsBottomTable = playersBottomTable
            .GetComponentsInChildren<PlayerCardManager>()
            .ToList();
        cardsBottomTable.Reverse();

        StartCoroutine(CardsDisplay(cardsTopTable));
        StartCoroutine(CardsDisplay(cardsBottomTable));
        StartCoroutine(Countdown());
        yield return new WaitForSeconds(TIME_UNTIL_GAME_STARTS);
        playersContainer.GetComponent<CanvasGroup>().DOFade(0, .5f);
    }

    IEnumerator SurviveAnimation()
    {
        surviveContainer.GetComponent<CanvasGroup>().DOFade(1, .5f);
        yield return new WaitForSeconds(2f);
        surviveContainer.GetComponent<CanvasGroup>().DOFade(0, .5f);
    }

    IEnumerator CardsDisplay(List<PlayerCardManager> playersCardList)
    {
        foreach (PlayerCardManager card in playersCardList)
        {
            card.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
            yield return new WaitForSeconds(.1f);
        }
    }

    IEnumerator Countdown()
    {
        for (int i = 0; i < TIME_UNTIL_GAME_STARTS; i++)
        {
            countDown.text = (TIME_UNTIL_GAME_STARTS - i).ToString();
            yield return new WaitForSeconds(1f);
        }
    }

    void PulseAnimation(GameObject objectToAnimate, float originalScale)
    {
        objectToAnimate.transform
            .DOScale(originalScale + .05f, 1f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.Linear);
    }

    void GeneratePlayersList()
    {
        SocketConnectionManager.Instance.players.ForEach(
            (player) =>
            {
                if (SocketConnectionManager.Instance.players.IndexOf(player) < 5)
                {
                    GameObject item = Instantiate(playerCard, playersTopTable.transform);
                    item.GetComponent<PlayerCardManager>().playerName.text = player.name;
                    if (player == Utils.GetPlayer(SocketConnectionManager.Instance.playerId))
                    {
                        item.GetComponent<PlayerCardManager>().youTag.SetActive(true);
                    }
                }
                else if (SocketConnectionManager.Instance.players.IndexOf(player) >= 5)
                {
                    GameObject item = Instantiate(playerCard, playersBottomTable.transform);
                    item.GetComponent<PlayerCardManager>().playerName.text = player.name;
                    if (player == Utils.GetPlayer(SocketConnectionManager.Instance.playerId))
                    {
                        item.GetComponent<PlayerCardManager>().youTag.SetActive(true);
                    }
                }
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
