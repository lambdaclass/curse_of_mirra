using UnityEngine;

public class DeathSplashDefeaterCharacter : MonoBehaviour
{
    private void OnEnable()
    {
        gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = GetDefeaterCharacter();
    }

    private string GetDefeaterCharacter()
    {
        // TODO: get defeater character
        return "-";
    }
}
