using UnityEngine;

public class DeathSplashKilledBy : MonoBehaviour
{
    private void Awake()
    {
        gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = GetKiller();
    }

    private string GetKiller()
    {
        // TODO: get killer
        return "-";
    }
}
