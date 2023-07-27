using UnityEngine;

public class DeathSplashKilledBy : MonoBehaviour
{
    private void Awake()
    {
        var killer = GetKiller();
        gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = killer.ToString();
    }

    private int GetKiller()
    {
        // get killer
        return 0;
    }
}
