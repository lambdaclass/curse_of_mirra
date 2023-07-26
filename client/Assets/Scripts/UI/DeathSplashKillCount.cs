using System.Linq;
using UnityEngine;

public class DeathSplashKillCount : MonoBehaviour
{
    private void Awake()
    {
        var killCount = GetKillCount();
        gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Total kills: " + killCount;
    }

    private int GetKillCount()
    {
        // get kill count
        return 0;
    }
}
