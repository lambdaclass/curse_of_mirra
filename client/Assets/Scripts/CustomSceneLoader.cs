using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.UI;
using System;

public class CustomSceneLoader : MMSceneLoadingManager
{
    [SerializeField]
    private AssetLabelReference labelReference;

    public List<string> keys = new List<string>() { "logo", "minion", "lobby" };

    public List<AssetLabelReference> labels = new List<AssetLabelReference>();

    // Operation handle used to load and release assets
    AsyncOperationHandle<IList<GameObject>> loadHandle;

    public static Sprite logoImage;

    public static Sprite minionImage;

    public static CustomSceneLoader Instance;
    uint done = 0;

    protected override void Start()
    {
        Addressables.InitializeAsync().Completed += Addressables_Completed;
        StartCoroutine(LoadAssets());
        base.Start();
    }

    private void Addressables_Completed(AsyncOperationHandle<IResourceLocator> handle)
    {
        labels.ForEach(
            (assetLabel) =>
            {
                Addressables.LoadAssetAsync<Sprite>(assetLabel).Completed += (
                    asyncOperationHandle
                ) =>
                {
                    if (asyncOperationHandle.IsDone)
                    {
                        print(asyncOperationHandle.Result + " Loaded successfully ");
                        print("height " + asyncOperationHandle.Result.rect.height);
                        done++;
                    }
                };
            }
        );
    }

    private IEnumerator LoadAssets()
    {
        yield return new WaitUntil(() => done == labels.Count);
        UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScreen");
    }
}
