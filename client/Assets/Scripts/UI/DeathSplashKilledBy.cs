using System.Linq;
using UnityEngine;

public class DeathSplashKilledBy : MonoBehaviour
{
    private void Awake()
    {
        var killer = GetKiller();
        gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "You were killed by: " + killer;
    }

    private int GetKiller()
    {
        // get killer
        return 0;
    }
}
