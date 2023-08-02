using System.Linq;
using UnityEngine;

public class DeathSplashEndMessage : MonoBehaviour
{
    private const string WINNER_MESSAGE = "THE KING OF ARABAN!";
    private const string LOSER_MESSAGE = "BETTER LUCK NEXT TIME!";

    private void OnEnable()
    {
        var endMessage = ThisPlayerIsWinner() ? WINNER_MESSAGE : LOSER_MESSAGE;
        gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = endMessage;
    }

    private bool ThisPlayerIsWinner()
    {
        return SocketConnectionManager.Instance.winnerPlayer.Item1 != null
            && SocketConnectionManager.Instance.winnerPlayer.Item1.Id
                == LobbyConnection.Instance.playerId;
    }
}
