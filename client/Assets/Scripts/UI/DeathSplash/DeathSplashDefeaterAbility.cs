using UnityEngine;

public class DeathSplashDefeaterAbility : MonoBehaviour
{
    private void Awake()
    {
        gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = GetDefeaterAbility();
    }

    private string GetDefeaterAbility()
    {
        // TODO: get defeater ability
        return "-";
    }
}
