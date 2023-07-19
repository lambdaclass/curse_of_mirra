using System.Linq;
using UnityEngine;

public class DeathSplashRanking : MonoBehaviour
{
    private void Awake()
    {
        var ranking = GetRanking();
        gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = ranking.ToString();
    }

    private int GetRanking()
    {
        return GetAlivePlayers() + 1;
    }

    private int GetAlivePlayers()
    {
        return SocketConnectionManager.Instance.gamePlayers
            .Where(player => player.Status == 0)
            .Count();
    }
}
