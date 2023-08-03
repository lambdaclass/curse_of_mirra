using UnityEngine;
using UnityEngine.UI;

public class DeathSplashDefeaterImage : MonoBehaviour
{
    private void OnEnable()
    {
        gameObject.GetComponent<Image>().sprite = GetDefeaterSprite();
    }

    private Sprite GetDefeaterSprite()
    {
        // TODO: get defeater sprite
        return null;
    }
}
