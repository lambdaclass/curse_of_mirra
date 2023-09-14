using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets.ResourceLocators;
using System;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.ResourceLocations;

public class TitleScreenController : MonoBehaviour
{
    private const string TITLE_SCENE_NAME = "Lobbies";

    [SerializeField]
    private CanvasGroup playNowButton;

    [SerializeField]
    public Image logoImage;

    [SerializeField]
    private AssetReference reference;

    void Start()
    {
        bool success = Caching.ClearCache();
        logoImage.sprite = Addressables.LoadAssetAsync<Sprite>(reference).Result;
        if (success)
        {
            // print("UNABLE TO CLEAR");
            print(logoImage.sprite.texture.name);
        }
        StartCoroutine(FadeIn(logoImage.GetComponent<CanvasGroup>(), 1f, .1f));
        StartCoroutine(FadeIn(playNowButton, .3f, 1.2f));
    }

    public void PlayNow()
    {
        SceneManager.LoadScene(TITLE_SCENE_NAME);
    }

    IEnumerator FadeIn(CanvasGroup element, float time, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (element != null)
        {
            for (float i = 0; i <= 1; i += Time.deltaTime / time)
            {
                element.alpha = i;
                yield return null;
            }
        }
    }
}
