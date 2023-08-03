using UnityEngine;

public class DeathSplashDefeaterAbility : MonoBehaviour
{
    private void OnEnable()
    {
        gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = GetDefeaterAbility();
    }

    private string GetDefeaterAbility()
    {
        // TODO: get defeater ability
        return "-";
    }
}
