using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Coffee.UIEffects;
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
        surviveTextContainer,
        playerCard,
        playersTopTable,
        playersBottomTable,
        prepareCoin,
        surviveText,
        smokeEffectBehind,
        smokeEffectFront;

    [SerializeField]
    TextMeshProUGUI countDown;

    [SerializeField]
    CinemachineVirtualCamera cinemachineVirtualCamera;
    CinemachineFramingTransposer cameraFramingTransposer;

    Vector3 cameraOffsetToCenterPosition = new Vector3(0, 30f, -18);
    GameObject player;
    const float CAMERA_START_OFFSET = 30f;
    const float PREPARE_FOR_BATTLE_DURATION = 3f;
    const float CHARACTERS_DISPLAY_DURATION = 5f;
    const float TIME_UNTIL_GAME_STARTS = 5f;
    const float SURVIVE_DURATION = 4f;
    float originalCountdownScale,
        originalCoinScale,
        originalCardScale,
        originalSurviveScale,
        originalCardYPosition;

    void Start()
    {
        cameraFramingTransposer =
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        originalCountdownScale = countDown.transform.localScale.x;
        originalCoinScale = prepareCoin.transform.localScale.x;
        originalCardScale = playerCard
            .GetComponent<PlayerCardManager>()
            .card
            .transform
            .localScale
            .x;
        originalSurviveScale = surviveTextContainer.transform.localScale.x;
        StartCoroutine(CameraCinematic());
    }

    IEnumerator CameraCinematic()
    {
        yield return new WaitUntil(() => GameServerConnectionManager.Instance.players.Count > 0);
        player = Utils.GetPlayer(GameServerConnectionManager.Instance.playerId);
        cinemachineVirtualCamera.ForceCameraPosition(
            CameraStartPosition(),
            cinemachineVirtualCamera.transform.rotation
        );
        GeneratePlayersList();
        loadingScreen.GetComponent<CanvasGroup>().DOFade(0, .1f);
        StartCoroutine(PrepareForBattleAnimation());
        yield return new WaitForSeconds(PREPARE_FOR_BATTLE_DURATION + 1f);
        StartCoroutine(PlayersAnimation());
        yield return new WaitForSeconds(CHARACTERS_DISPLAY_DURATION);
        StartCoroutine(SurviveAnimation());
        yield return new WaitForSeconds(SURVIVE_DURATION);
        GetComponent<CanvasGroup>().DOFade(0, .5f);
        battleScreen.GetComponent<CanvasGroup>().DOFade(1, .5f);
        gameObject.SetActive(false);
    }

    IEnumerator PrepareForBattleAnimation()
    {
        prepareBattleContainer.GetComponent<CanvasGroup>().alpha = 1f;
        CoinDisplayAnimation(prepareCoin, originalCoinScale);
        yield return new WaitForSeconds(1f);
        prepareCoin.GetComponent<UIShiny>().enabled = true;
        prepareCoin.GetComponent<Animator>().enabled = true;
        cinemachineVirtualCamera
            .transform
            .DOMove(
                player.transform.position + cameraOffsetToCenterPosition,
                PREPARE_FOR_BATTLE_DURATION
            )
            .SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(PREPARE_FOR_BATTLE_DURATION);
        cinemachineVirtualCamera.ForceCameraPosition(
            player.transform.position + cameraOffsetToCenterPosition,
            cinemachineVirtualCamera.transform.rotation
        );
        SetCameraToPlayer(GameServerConnectionManager.Instance.playerId);
        prepareBattleContainer.GetComponent<CanvasGroup>().alpha = 0;
    }

    IEnumerator PlayersAnimation()
    {
        playersContainer.GetComponent<CanvasGroup>().DOFade(1, .5f);
        List<PlayerCardManager> cardsTopTable = playersTopTable
            .GetComponentsInChildren<PlayerCardManager>()
            .ToList();
        List<PlayerCardManager> cardsBottomTable = playersBottomTable
            .GetComponentsInChildren<PlayerCardManager>()
            .ToList();
        cardsBottomTable.Reverse();
        StartCoroutine(CardsDisplay(cardsTopTable, 1));
        StartCoroutine(CardsDisplay(cardsBottomTable, -1));
        StartCoroutine(Countdown());
        yield return new WaitForSeconds(CHARACTERS_DISPLAY_DURATION);
        playersContainer.GetComponent<CanvasGroup>().DOFade(0, .1f);
    }

    IEnumerator SurviveAnimation()
    {
        surviveContainer.GetComponent<CanvasGroup>().DOFade(1, .1f);
        smokeEffectBehind.SetActive(true);
        surviveTextContainer.transform.DOScale(originalSurviveScale + 1f, .4f);
        yield return new WaitForSeconds(1.75f);
        surviveText.GetComponent<CanvasGroup>().DOFade(0, .2f);
        smokeEffectBehind.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        surviveContainer.GetComponent<CanvasGroup>().DOFade(0, .1f);
    }

    IEnumerator CardsDisplay(List<PlayerCardManager> playersCardList, int multiplier)
    {
        foreach (PlayerCardManager cardContainer in playersCardList)
        {
            originalCardYPosition = cardContainer.card.transform.position.y;
            cardContainer.card.transform.position = new Vector3(
                cardContainer.card.transform.position.x,
                originalCardYPosition + 100f * multiplier,
                cardContainer.card.transform.position.z
            );
            Sequence displaySequence = DOTween.Sequence();
            displaySequence
                .Append(
                    cardContainer
                        .card
                        .transform
                        .DOMoveY(originalCardYPosition - 20f * multiplier, 0.25f)
                )
                .Append(cardContainer.card.transform.DOMoveY(originalCardYPosition, 0.25f))
                .Insert(0, cardContainer.card.GetComponent<CanvasGroup>().DOFade(1, 0.5f));
            yield return new WaitForSeconds(.15f);
        }
        yield return new WaitForSeconds(CHARACTERS_DISPLAY_DURATION - 1.5f);
        foreach (PlayerCardManager cardContainer in playersCardList)
        {
            originalCardYPosition = cardContainer.card.transform.position.y;
            Sequence hideDisplaySequence = DOTween.Sequence();
            hideDisplaySequence
                .Append(
                    cardContainer
                        .card
                        .transform
                        .DOMoveY(originalCardYPosition + 100 * multiplier, 0.15f)
                )
                .Insert(0, cardContainer.card.GetComponent<CanvasGroup>().DOFade(0, 0.25f));
            yield return new WaitForSeconds(.1f);
        }
    }

    IEnumerator Countdown()
    {
        Sequence displaySequence = DOTween.Sequence();
        displaySequence
            .Append(countDown.transform.DOScale(originalCountdownScale + 0.2f, .5f))
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.Linear);
        for (int i = 0; i < TIME_UNTIL_GAME_STARTS; i++)
        {
            countDown.text = (TIME_UNTIL_GAME_STARTS - i).ToString();

            yield return new WaitForSeconds(1f);
        }
    }

    Vector3 CameraStartPosition()
    {
        float xPosition;
        float zPosition;
        if (player.transform.position.z > 0)
        {
            zPosition = player.transform.position.z - CAMERA_START_OFFSET;
        }
        else
        {
            zPosition = player.transform.position.z + CAMERA_START_OFFSET;
        }
        if (player.transform.position.x > 0)
        {
            xPosition = player.transform.position.x - CAMERA_START_OFFSET;
        }
        else
        {
            xPosition = player.transform.position.x + CAMERA_START_OFFSET;
        }
        Vector3 playerPosition = new Vector3(xPosition, player.transform.position.y, zPosition);
        return playerPosition + cameraOffsetToCenterPosition;
    }

    void CoinDisplayAnimation(GameObject objectToAnimate, float originalScale)
    {
        Sequence stickerSequence = DOTween.Sequence();
        stickerSequence
            .Append(objectToAnimate.GetComponent<CanvasGroup>().DOFade(1, .3f))
            .Insert(0, objectToAnimate.transform.DOScale(originalScale + .05f, .3f))
            .Append(objectToAnimate.transform.DOScale(originalScale, .3f))
            .PrependInterval(.2f)
            .SetEase(Ease.InQuad);
    }

    void GeneratePlayersList()
    {
        GameServerConnectionManager
            .Instance
            .players
            .ForEach(
                (player) =>
                {
                    if (GameServerConnectionManager.Instance.players.IndexOf(player) < 5)
                    {
                        GameObject item = Instantiate(playerCard, playersTopTable.transform);
                        item.GetComponent<PlayerCardManager>().playerName.text = player.name;

                        // prefab = charactersInfo.Find(el => el.name == player.CharacterName).prefab;
                        foreach (
                            CoMCharacter character in CharactersManager.Instance.AvailableCharacters
                        )
                        {
                            if (
                                character.name
                                == player.GetComponent<CustomCharacter>().CharacterModel.name
                            )
                            {
                                Sprite characterSprite = character.battleCharacterCard;
                            }
                        }

                        if (
                            player == Utils.GetPlayer(GameServerConnectionManager.Instance.playerId)
                        )
                        {
                            item.GetComponent<PlayerCardManager>().youTag.SetActive(true);
                        }
                    }
                    else if (GameServerConnectionManager.Instance.players.IndexOf(player) >= 5)
                    {
                        GameObject item = Instantiate(playerCard, playersBottomTable.transform);
                        item.GetComponent<PlayerCardManager>().playerName.text = player.name;
                        if (
                            player == Utils.GetPlayer(GameServerConnectionManager.Instance.playerId)
                        )
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
