using UnityEngine;

public class DeathSplashDefeatedBy : MonoBehaviour
{
    private void Awake()
    {
        var killCount = GetKillCount();
        gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = killCount.ToString() + "KILLS";
    }

    private int GetKillCount()
    {
        // get kill count
        return 0;
    }
}
