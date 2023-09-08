using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets.ResourceLocators;
using System;
using UnityEngine.Networking;

public class TitleScreenController : MonoBehaviour
{
    private const string TITLE_SCENE_NAME = "Lobbies";

    [SerializeField]
    private CanvasGroup playNowButton;

    [SerializeField]
    public Image logoImage;

    [SerializeField]
    private AssetLabelReference logoReference;

    [SerializeField]
    private AssetReference reference;

    void Start()
    {
        bool success = Caching.ClearCache();
        if (success)
        {
            // print("UNABLE TO CLEAR");
            print(logoImage.sprite.texture.name);
        }
        Addressables.InitializeAsync().Completed += Addressables_Completed;
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

    private void Addressables_Completed(AsyncOperationHandle<IResourceLocator> handle)
    {
        Addressables.LoadAssetAsync<Sprite>(reference).Completed += (asyncOperationHandle) =>
        {
            Sprite sprite = asyncOperationHandle.Result;
            logoImage.sprite = sprite;
            print("Asset loaded successfully");
            print("Height " + sprite.texture.height);
        };

        // Addressables.LoadAssetAsync<Sprite>(logoReference).Completed += (asyncOperationHandle) =>
        // {
        //     Sprite logoSprite = asyncOperationHandle.Result;
        //     print("Loading Scene...");
        //     print(logoSprite.texture.height);
        //     logoImage.sprite = logoSprite;
        // };
    }
}
