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
        loadingIcon,
        prepareBattleContainer,
        playersContainer,
        surviveContainer,
        surviveTextContainer,
        playerCard,
        darkOvevrlay,
        playersTopTable,
        playersBottomTable,
        prepareCoin,
        surviveText;

    [SerializeField]
    TextMeshProUGUI countDown;

    [SerializeField]
    CinemachineVirtualCamera cinemachineVirtualCamera;
    Vector3 cameraDistanceFromGround = new Vector3(0, 30f, -18);
    Vector3 playerPosition;
    GameObject player;
    const float CAMERA_START_OFFSET = 30f;
    const float PREPARE_FOR_BATTLE_DURATION = 3f;
    const float CHARACTERS_DISPLAY_DURATION = 4f;
    float TIME_UNTIL_GAME_STARTS = 0f;
    const float SURVIVE_DURATION = 1.65f;

    bool countdownDone = false;

    float originalCountdownScale,
        originalCoinScale,
        originalSurviveScale,
        originalCardYPosition;
    bool loadingComplete = false;

    void Start()
    {
        originalCountdownScale = countDown.transform.localScale.x;
        originalCoinScale = prepareCoin.transform.localScale.x;
        originalSurviveScale = surviveTextContainer.transform.localScale.x;
        StartCoroutine(CameraCinematic());
    }

    IEnumerator CameraCinematic()
    {
        StartCoroutine(LoadingAnimation());
        yield return new WaitUntil(
            () => GameServerConnectionManager.Instance.players.Count > 0 && loadingComplete
        );
                player = Utils.GetPlayer(GameServerConnectionManager.Instance.playerId);
        Position playerBackEndPosition = Utils
            .GetGamePlayer(GameServerConnectionManager.Instance.playerId)
            .Position;
        playerPosition = Utils.transformBackendOldPositionToFrontendPosition(playerBackEndPosition);
        GeneratePlayersList();
        cinemachineVirtualCamera.ForceCameraPosition(
            CameraStartPosition(),
            cinemachineVirtualCamera.transform.rotation
        );
        loadingScreen.GetComponent<CanvasGroup>().DOFade(0, .1f);
        StartCoroutine(PrepareForBattleAnimation());
        yield return new WaitForSeconds(PREPARE_FOR_BATTLE_DURATION + 1f);
        StartCoroutine(PlayersAnimation());
        yield return new WaitUntil(
            () => countdownDone
        );
        StartCoroutine(SurviveAnimation());
        yield return new WaitForSeconds(SURVIVE_DURATION);
        gameObject.SetActive(false);
    }

    IEnumerator LoadingAnimation()
    {
        Sequence loadingSequence = DOTween.Sequence();
        loadingSequence
            .Append(loadingIcon.transform.DORotate(new Vector3(0, 0, -180), 0.5f))
            .Append(loadingIcon.transform.DORotate(new Vector3(0, 0, -360), 0.5f))
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.InOutQuart);
        yield return new WaitForSeconds(1.5f);
        loadingComplete = true;
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
            .DOMove(playerPosition + cameraDistanceFromGround, PREPARE_FOR_BATTLE_DURATION)
            .SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(PREPARE_FOR_BATTLE_DURATION);
        cinemachineVirtualCamera.ForceCameraPosition(
            playerPosition + cameraDistanceFromGround,
            cinemachineVirtualCamera.transform.rotation
        );
        SetCameraToPlayer(GameServerConnectionManager.Instance.playerId);
        prepareBattleContainer.GetComponent<CanvasGroup>().alpha = 0;
    }

    IEnumerator PlayersAnimation()
    {
        darkOvevrlay.GetComponent<CanvasGroup>().DOFade(1, .25f);
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
        yield return new WaitUntil(
            () => countdownDone
        );
        playersContainer.GetComponent<CanvasGroup>().DOFade(0, .1f);
    }

    IEnumerator SurviveAnimation()
    {
        surviveContainer.GetComponent<CanvasGroup>().DOFade(1, .1f);
        surviveTextContainer.transform.DOScale(originalSurviveScale + 1.5f, .4f);
        yield return new WaitForSeconds(.6f);
        surviveText.GetComponent<CanvasGroup>().DOFade(0, .1f);
        yield return new WaitForSeconds(.3f);
        surviveContainer.GetComponent<CanvasGroup>().DOFade(0, .25f);
        GetComponent<CanvasGroup>().DOFade(0, .25f);
        battleScreen.GetComponent<CanvasGroup>().DOFade(1, .25f);
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
                    TIME_UNTIL_GAME_STARTS =
            (int)(GameServerConnectionManager.Instance.gameCountdown / 1000) - 1;
        for (int i = 0; i < TIME_UNTIL_GAME_STARTS; i++)
        {
            countDown.text = (TIME_UNTIL_GAME_STARTS - i).ToString();
            yield return new WaitForSeconds(1f);
        }
        countdownDone = true;
    }

    Vector3 CameraStartPosition()
    {
        float xPosition;
        float zPosition;
        if (playerPosition.z > 0)
        {
            zPosition = playerPosition.z - CAMERA_START_OFFSET;
        }
        else
        {
            zPosition = playerPosition.z + CAMERA_START_OFFSET;
        }
        if (playerPosition.x > 0)
        {
            xPosition = playerPosition.x - CAMERA_START_OFFSET;
        }
        else
        {
            xPosition = playerPosition.x + CAMERA_START_OFFSET;
        }
        Vector3 adjustedPlayerOffset = new Vector3(xPosition, playerPosition.y, zPosition);
        return adjustedPlayerOffset + cameraDistanceFromGround;
    }

    void CoinDisplayAnimation(GameObject coin, float originalScale)
    {
        Sequence stickerSequence = DOTween.Sequence();
        stickerSequence
            .AppendInterval(.3f)
            .Append(coin.GetComponent<CanvasGroup>().DOFade(1, .3f))
            .Insert(0, coin.transform.DOScale(originalScale + .05f, .3f))
            .Append(coin.transform.DOScale(originalScale, .3f))
            .SetEase(Ease.InQuad)
            .onComplete = () =>
        {
            AnimationCallback(coin.GetComponent<Animator>());
        };
    }

    void AnimationCallback(Animator objectToAnimate)
    {
        objectToAnimate.Play("Animation", 0, 0.0f);
    }

    void GeneratePlayersList()
    {
        GameServerConnectionManager
            .Instance
            .gamePlayers
            .ForEach(
                (player) =>
                {
                    var index = GameServerConnectionManager.Instance.gamePlayers.IndexOf(player);
                    GeneratePlayer(index, player);
                }
            );
    }

    void GeneratePlayer(int index, Entity player)
    {
        string characterName = Utils.GetCharacter(player.Id).CharacterModel.name;
        Transform pos = index < 5 ? playersTopTable.transform : playersBottomTable.transform;
        PlayerCardManager item = Instantiate(playerCard, pos).GetComponent<PlayerCardManager>();
        item.playerName.text = player.Name;

        if (player.Id == GameServerConnectionManager.Instance.playerId)
        {
            item.youTag.SetActive(true);
        }
        Sprite characterIcon = CharactersManager
            .Instance
            .AvailableCharacters
            .Where(
                character =>
                    character.name == characterName
            )
            .Single()
            .battleCharacterCard;
        item.character.sprite = characterIcon;
    }

    private void SetCameraToPlayer(ulong playerID)
    {
        foreach (CustomCharacter player in CustomLevelManager.Instance.PlayerPrefabs)
        {
            if (UInt64.Parse(player.PlayerID) == playerID)
            {
                CinemachineCameraController cinemachineController =
                    cinemachineVirtualCamera.GetComponent<CinemachineCameraController>();
                cinemachineController.SetTarget(player);
                cinemachineController.StartFollowing();
            }
        }
    }
}
