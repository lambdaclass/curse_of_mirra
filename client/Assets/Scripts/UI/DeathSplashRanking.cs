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
        return Utils.GetAlivePlayers().Count() + 1;
    }
}
