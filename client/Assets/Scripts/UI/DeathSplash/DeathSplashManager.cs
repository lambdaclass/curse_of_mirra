using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Collections.Generic;

public class DeathSplashManager : MonoBehaviour
{
    [SerializeField]
    GameObject backgroundEndGame;

    [SerializeField]
    SpectateManager spectateManager;

    [SerializeField]
    TextMeshProUGUI title;

    [SerializeField]
    TextMeshProUGUI winnerName;

    [SerializeField]
    TextMeshProUGUI winnerCharacter;

    [SerializeField]
    TextMeshProUGUI rankingText;

    [SerializeField]
    TextMeshProUGUI messageText;

    [SerializeField]
    TextMeshProUGUI amountOfKillsText;

    [SerializeField]
    GameObject defeatedByContainer;

    [SerializeField]
    TextMeshProUGUI defeater;

    [SerializeField]
    Image defeaterImage;

    [SerializeField]
    TextMeshProUGUI defeaterName;

    [SerializeField]
    TextMeshProUGUI defeaterAbility;

    [SerializeField]
    GameObject characterModelContainer;

    [SerializeField]
    List<GameObject> characterModels;

    private const int WINNER_POS = 1;
    private const int SECOND_PLACE_POS = 2;
    private const string WINNER_MESSAGE = "THE KING OF ARABAN!";
    private const string LOSER_MESSAGE = "BETTER LUCK NEXT TIME!";
    GameObject player;
    GameObject modelClone;

    public void SetDeathSplashPlayer()
    {
        player = Utils.GetPlayer(SocketConnectionManager.Instance.playerId);
        GameObject characterModel = characterModels.Single(
            characterModel =>
                characterModel.name.Contains(
                    player.GetComponent<CustomCharacter>().CharacterModel.name
                )
        );
        modelClone = Instantiate(characterModel, characterModelContainer.transform);
    }

    void OnEnable()
    {
        ShowRankingDisplay();
        ShowMessage();
        ShowMatchInfo();
        ShowPlayerAnimation();
    }

    void ShowRankingDisplay()
    {
        var ranking = GetRanking();
        rankingText.text = "# " + ranking.ToString();
    }

    private int GetRanking()
    {
        bool isWinner = SocketConnectionManager.Instance.PlayerIsWinner(
            SocketConnectionManager.Instance.playerId
        );

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

    void ShowMessage()
    {
        var endGameMessage = SocketConnectionManager.Instance.PlayerIsWinner(
            SocketConnectionManager.Instance.playerId
        )
            ? WINNER_MESSAGE
            : LOSER_MESSAGE;
        messageText.text = endGameMessage;
    }

    void ShowMatchInfo()
    {
        // Kill count
        var killCount = GetKillCount();
        var killCountMessage = killCount == 1 ? " KILL" : " KILLS";
        amountOfKillsText.text = killCount.ToString() + killCountMessage;
        // This conditional should be activated when the info needed is ready
        /* if (!PlayerIsWinner())
        {
            defeatedByContainer.SetActive(true);
        } */
        // Defeated By
        defeater.text = GetDefeater();
        // Defeated By Image
        defeaterImage.sprite = GetDefeaterSprite();
        // Defeated By Name
        defeaterName.text = GetDefeaterCharacter();
        // Defeated By Ability
        defeaterAbility.text = GetDefeaterAbility();
    }

    private ulong GetKillCount()
    {
        var playerId = SocketConnectionManager.Instance.playerId;
        var gamePlayer = Utils.GetGamePlayer(playerId);
        return gamePlayer.KillCount;
    }

    private string GetDefeater()
    {
        // TODO: get Defeater
        return "-";
    }

    private Sprite GetDefeaterSprite()
    {
        // TODO: get defeater sprite
        return null;
    }

    private string GetDefeaterCharacter()
    {
        // TODO: get defeater character
        return "-";
    }

    private string GetDefeaterAbility()
    {
        // TODO: get defeater ability
        return "-";
    }

    private void ShowPlayerAnimation()
    {
        if (player)
        {
            if (
                SocketConnectionManager.Instance.PlayerIsWinner(
                    SocketConnectionManager.Instance.playerId
                )
            )
            {
                modelClone.GetComponentInChildren<Animator>().SetBool("Victory", true);
            }
            else
            {
                modelClone.GetComponentInChildren<Animator>().SetBool("Defeat", true);
            }
        }
    }

    public void ShowEndGameScreen()
    {
        // TODO: get image from lobby
        backgroundEndGame.SetActive(true);
        spectateManager.UnsetSpectateMode();

        string playerName = LobbyConnection.Instance.playersIdName[
            SocketConnectionManager.Instance.winnerPlayer.Item1.Id
        ];
        winnerName.text = playerName;
        winnerCharacter.text = SocketConnectionManager.Instance.winnerPlayer.Item1.CharacterName;
        if (
            SocketConnectionManager.Instance.PlayerIsWinner(
                SocketConnectionManager.Instance.playerId
            )
        )
        {
            title.text = "Victory";
        }
        else
        {
            title.text = "Defeat";
        }
    }
}
