using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EndGameManager : MonoBehaviour
{
    [SerializeField]
    public GameObject finalSplash;

    [SerializeField]
    TextMeshProUGUI rankingText,
        amountOfKillsText,
        defeaterPlayerName;

    [SerializeField] 
    Image rankIcon;

    [SerializeField]
    GameObject defeatedByContainer;

    [SerializeField]
    Image defeaterImage;

    [SerializeField]
    public UIModelManager modelManager;

    [SerializeField] 
    GameObject winnerContainer;

    [SerializeField]
    TextMeshProUGUI winnerNameText;

    [SerializeField]
    TextMeshProUGUI winnerCharacterText;

    [SerializeField] 
    private Sprite goldRank;

    [SerializeField] 
    private Sprite silverRank;

    [SerializeField] 
    private Sprite bronzeRank;

    [SerializeField]
    private Bounty bounty;

    private const int WINNER_POS = 1;
    private const int SECOND_PLACE_POS = 2;
    private const ulong ZONE_ID = 9999;
    CustomCharacter player;

    void OnEnable()
    {
        ShowRankingDisplay();
        ShowMatchInfo();
        bounty.SetBounty(GameServerConnectionManager.Instance.bountySelected);
    }

    public void SetDeathSplashCharacter()
    {
        player = Utils.GetCharacter(GameServerConnectionManager.Instance.playerId);

        CoMCharacter character = CharactersManager
            .Instance
            .AllCharacters
            .Single(characterSO => characterSO.name.Contains(player.CharacterModel.name));

        if (character)
        {
            modelManager.SetModel(character.name);
        }
    }

    public void ShowWinner()
    {
        winnerContainer.GetComponent<CanvasGroup>().DOFade(1, 1f);
        winnerNameText.text = GameServerConnectionManager.Instance.winnerPlayer.Item1.Name;
        winnerCharacterText.text = GameServerConnectionManager.Instance.winnerPlayer.Item1.Player.CharacterName;
    }

    void ShowRankingDisplay()
    {
        var ranking = GetRanking();

        switch (ranking)
        {
            case 1: 
                rankIcon.sprite  = goldRank;
                break;
            case 2: 
                rankIcon.sprite  = silverRank;
                break;
            case 3: 
                rankIcon.sprite  = bronzeRank;
                break;
        }

        rankingText.text = ranking.ToString();
    }

    private int GetRanking()
    {
        bool isWinner = GameServerConnectionManager
            .Instance
            .PlayerIsWinner(GameServerConnectionManager.Instance.playerId);

        // FIXME This is a temporal for the cases where the winner dies simultaneously
        // FIXME with other/s player/s
        if (isWinner)
        {
            return WINNER_POS;
        }
        if (Utils.GetAlivePlayers().Count() == 0)
        {
            return SECOND_PLACE_POS;
        }
        return Utils.GetAlivePlayers().Count() + 1;
    }

    void ShowMatchInfo()
    {
        // Kill count
        amountOfKillsText.text = Utils
            .GetGamePlayer(GameServerConnectionManager.Instance.playerId)
            ?.Player
            .KillCount
            .ToString();

        // Defeated By
        if (
            player
            && GameServerConnectionManager
                .Instance
                .PlayerIsWinner(GameServerConnectionManager.Instance.playerId)
        )
        {
            defeatedByContainer.SetActive(false);
        }
        else
        {
            MaybeShowDefeaterName();
            // Defeated By Image
            defeaterImage.sprite = GetDefeaterSprite();
        }
    }

    private ulong GetKillCount()
    {
        // var playerId = GameServerConnectionManager.Instance.playerId;
        // var gamePlayer = Utils.GetGamePlayer(playerId);
        // return gamePlayer.KillCount;
        return 77;
    }

    void MaybeShowDefeaterName()
    {
        if (KillFeedManager.instance.GetMyKillerId() == ZONE_ID)
        {
            defeaterPlayerName.gameObject.SetActive(false);
        }
        else
        {
            defeaterPlayerName.text = GetDefeaterPlayerName();
        }
    }

    private Sprite GetDefeaterSprite()
    {
        if (KillFeedManager.instance.GetMyKillerId() == ZONE_ID)
        {
            return KillFeedManager.instance.zoneIcon;
        }
        else
        {
            CoMCharacter killerCharacter = CharactersManager
                .Instance
                .AvailableCharacters
                .Single(
                    characterSO =>
                        characterSO
                            .name
                            .Contains(
                                Utils
                                    .GetPlayer(KillFeedManager.instance.GetMyKillerId())
                                    .GetComponent<CustomCharacter>()
                                    .CharacterModel
                                    .name
                            )
                );
            return killerCharacter.UIIcon;
        }
    }

    private string GetDefeaterPlayerName()
    {
        return Utils.GetGamePlayer(KillFeedManager.instance.GetMyKillerId()).Name;
    }
}
