using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameManager : MonoBehaviour
{
    [SerializeField]
    public GameObject finalSplash;

    [SerializeField]
    TextMeshProUGUI rankingText,
        rankingTextShadow,
        amountOfKillsText;

    [SerializeField]
    GameObject defeatedByContainer,
        characterModelContainer;

    [SerializeField]
    Image defeaterImage;

    private const int WINNER_POS = 1;
    private const int SECOND_PLACE_POS = 2;
    private const string ZONE_ID = "0";
    CustomCharacter player;
    GameObject modelClone;

    Animator modelAnimator;

    void OnEnable()
    {
        ShowRankingDisplay();
        ShowMatchInfo();
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
            GameObject characterModel = character.UIModel;
            modelClone = Instantiate(characterModel, characterModelContainer.transform);
            modelAnimator = modelClone.GetComponentInChildren<Animator>();
        }
    }

    void ShowRankingDisplay()
    {
        var ranking = GetRanking();
        rankingText.text += " # " + ranking.ToString();
        rankingTextShadow.text += " # " + ranking.ToString();
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
            ?.Player.KillCount
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
            //defeaterPlayerName.text = GetDefeaterPlayerName();
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

    private Sprite GetDefeaterSprite()
    {
        if (KillFeedManager.instance.myKillerId.ToString() == ZONE_ID)
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
                                    .GetPlayer(KillFeedManager.instance.myKillerId)
                                    .GetComponent<CustomCharacter>()
                                    .CharacterModel
                                    .name
                            )
                );
            return killerCharacter.UIIcon;
        }
    }

    public void ShowCharacterAnimation()
    {
        if (player)
        {
            if (
                GameServerConnectionManager
                    .Instance
                    .PlayerIsWinner(GameServerConnectionManager.Instance.playerId)
            )
            {
                modelAnimator.SetBool("Victory", true);
            }
            else
            {
                 // Current workaround for muflus animation. Refactor have to be done in ticket #1516
                if(modelClone.name.ToLower().Contains("muflus")){
                    modelAnimator.Play("Defeat");   
                } else {
                    modelAnimator.SetBool("Defeat", true);
                }
            }
        }
    }
}
