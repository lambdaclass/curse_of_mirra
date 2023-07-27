using UnityEngine;

public class DeathSplashDefeaterCharacter : MonoBehaviour
{
    private void Awake()
    {
        var killCount = GetKillCount();
        gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = killCount.ToString() + "KILLS";
    }

    private int GetKillCount()
    {
        // get kill count
        // asdfas
        return 0;
    }
}
