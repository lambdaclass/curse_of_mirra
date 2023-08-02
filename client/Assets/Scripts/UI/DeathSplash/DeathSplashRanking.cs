using System.Linq;
using UnityEngine;

public class DeathSplashRanking : MonoBehaviour
{
    private void OnEnable()
    {
        var ranking = GetRanking();
        gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = ranking.ToString();
    }

    private int GetRanking()
    {
        if (ThisPlayerIsWinner())
        {
            print(SocketConnectionManager.Instance.winnerPlayer.Item1);
            return 1;
        }
        return Utils.GetAlivePlayers().Count() + 1;
    }

    private bool ThisPlayerIsWinner()
    {
        return SocketConnectionManager.Instance.winnerPlayer.Item1 != null
            && SocketConnectionManager.Instance.winnerPlayer.Item1.Id
                == LobbyConnection.Instance.playerId;
    }
}
