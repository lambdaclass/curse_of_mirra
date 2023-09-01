using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class DeathSplashManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI rankingText;

    [SerializeField]
    TextMeshProUGUI messageText;

    [SerializeField]
    TextMeshProUGUI amountOfKillsText;
    private const string WINNER_MESSAGE = "THE KING OF ARABAN!";
    private const string LOSER_MESSAGE = "BETTER LUCK NEXT TIME!";

    void OnEnable()
    {
        // Ranking
        var ranking = GetRanking();
        rankingText.text = "# " + ranking.ToString();
        // Message
        var endGameMessage = PlayerIsWinner() ? WINNER_MESSAGE : LOSER_MESSAGE;
        messageText.text = endGameMessage;
        // Kill count
        var killCount = GetKillCount();
        var killCountMessage = killCount == 1 ? " KILL" : " KILLS";
        amountOfKillsText.text = killCount.ToString() + killCountMessage;
    }

    private int GetRanking()
    {
        if (PlayerIsWinner())
        {
            return 1;
        }
        return Utils.GetAlivePlayers().Count() + 1;
    }

    private bool PlayerIsWinner()
    {
        return SocketConnectionManager.Instance.winnerPlayer.Item1 != null
            && SocketConnectionManager.Instance.winnerPlayer.Item1.Id
                == LobbyConnection.Instance.playerId;
    }

    private ulong GetKillCount()
    {
        var playerId = LobbyConnection.Instance.playerId;
        var gamePlayer = Utils.GetGamePlayer(playerId);
        return gamePlayer.KillCount;
    }
}
