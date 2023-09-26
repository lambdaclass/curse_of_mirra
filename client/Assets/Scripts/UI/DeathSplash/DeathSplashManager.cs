using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using MoreMountains.TopDownEngine;
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
    GameObject playerModelContainer;

    private const int WINNER_POS = 1;
    private const string WINNER_MESSAGE = "THE KING OF ARABAN!";
    private const string LOSER_MESSAGE = "BETTER LUCK NEXT TIME!";
    GameObject player;
    GameObject playerModel;
    GameObject modelClone;

    public void SetDeathSplashPlayer()
    {
        player = Utils.GetPlayer(LobbyConnection.Instance.playerId);
        GameObject playerModel = player.GetComponent<CustomCharacter>().CharacterModel;
        modelClone = Instantiate(
            playerModel,
            playerModelContainer.transform.position,
            playerModelContainer.transform.rotation,
            playerModelContainer.transform
        );
    }

    void OnEnable()
    {
        ShowRankingDisplay();
        ShowMessage();
        ShowMatchInfo();
        ShowPlayerAnimation();
        ShowEndGameScreen();
    }

    void ShowRankingDisplay()
    {
        var ranking = GetRanking();
        rankingText.text = "# " + ranking.ToString();
    }

    private int GetRanking()
    {
        bool isWinner = SocketConnectionManager.Instance.PlayerIsWinner(
            LobbyConnection.Instance.playerId
        );

        return isWinner ? WINNER_POS : Utils.GetAlivePlayers().Count() + 1;
    }

    void ShowMessage()
    {
        var endGameMessage = SocketConnectionManager.Instance.PlayerIsWinner(
            LobbyConnection.Instance.playerId
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
        var playerId = LobbyConnection.Instance.playerId;
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
            // TODO: get model sizes to make them look the same
            if (modelClone.name.Contains("H4ck"))
            {
                modelClone.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            }
            if (modelClone.name.Contains("Muflus"))
            {
                modelClone.transform.localScale = new Vector3(0.34f, 0.34f, 0.34f);
                modelClone.transform.localPosition = new Vector3(0f, -0.1f, 0f);
            }
            if (modelClone.name.Contains("Dagna"))
            {
                modelClone.transform.localScale = new Vector3(1f, 1f, 1f);
            }
            if (modelClone.name.Contains("Uma"))
            {
                modelClone.transform.localScale = new Vector3(1.45f, 1.45f, 1.45f);
                modelClone.transform.localPosition = new Vector3(0f, 0.14f, 0f);
            }
            if (SocketConnectionManager.Instance.PlayerIsWinner(LobbyConnection.Instance.playerId))
            {
                modelClone.GetComponent<Animator>().SetBool("Victory", true);
            }
            else
            {
                modelClone.GetComponent<Animator>().SetBool("Defeat", true);
            }
        }
    }

    public void ShowEndGameScreen()
    {
        // TODO: get image from lobby
        if (SocketConnectionManager.Instance.GameHasEnded())
        {
            backgroundEndGame.SetActive(true);
            spectateManager.UnsetSpectateMode();
            // TODO: get player name
            winnerName.text =
                "Player " + SocketConnectionManager.Instance.winnerPlayer.Item1.Id.ToString();
            winnerCharacter.text = SocketConnectionManager
                .Instance
                .winnerPlayer
                .Item1
                .CharacterName;
            if (SocketConnectionManager.Instance.PlayerIsWinner(LobbyConnection.Instance.playerId))
            {
                title.text = "Victory";
            }
            else
            {
                title.text = "Defeat";
            }
        }
        else
        {
            backgroundEndGame.SetActive(false);
        }
    }
}
