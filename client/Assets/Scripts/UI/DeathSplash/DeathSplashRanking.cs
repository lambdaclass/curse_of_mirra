using System.Linq;
using UnityEngine;

public class DeathSplashRanking : MonoBehaviour
{
    private void OnEnable()
    {
        var ranking = GetRanking();
        gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "# " + ranking.ToString();
    }

    private int GetRanking()
    {
        if (SocketConnectionManager.Instance.PlayerIsWinner(LobbyConnection.Instance.playerId))
        {
            print(SocketConnectionManager.Instance.winnerPlayer.Item1);
            return 1;
        }
        return Utils.GetAlivePlayers().Count() + 1;
    }
}
