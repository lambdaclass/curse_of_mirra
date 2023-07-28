using UnityEngine;
using UnityEngine.UI;

public class DeathSplashDefeaterImage : MonoBehaviour
{
    private void Awake()
    {
        gameObject.GetComponent<Image>().sprite = GetDefeaterSprite();
    }

    private Sprite GetDefeaterSprite()
    {
        // TODO: get defeater sprite
        return null;
    }
}
