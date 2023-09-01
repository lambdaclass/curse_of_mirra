using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class DeathSplashManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI amountOfKills;

    // Start is called before the first frame update
    void Start()
    {
        var ranking = GetRanking();
        amountOfKills.text = "# " + ranking.ToString();
    }

    private int GetRanking()
    {
        if (PlayerIsWinner())
        {
            return 1;
        }
        else
        {
            return Utils.GetAlivePlayers().Count() + 1;
        }
    }

    private bool PlayerIsWinner()
    {
        return SocketConnectionManager.Instance.winnerPlayer.Item1 != null
            && SocketConnectionManager.Instance.winnerPlayer.Item1.Id
                == LobbyConnection.Instance.playerId;
    }
}
