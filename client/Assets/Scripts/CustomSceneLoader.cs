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

    public List<AssetLabelReference> labels = new List<AssetLabelReference>();

    uint done = 0;

    protected override void Start()
    {
        StartCoroutine(CheckForUpdates());
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

    public static IEnumerator CheckForUpdates()
    {
        print("Checking for updates...");
        List<string> catalogsToUpdate = new List<string>();
        AsyncOperationHandle<List<string>> checkForUpdateHandle =
            Addressables.CheckForCatalogUpdates();
        checkForUpdateHandle.Completed += op =>
        {
            catalogsToUpdate.AddRange(op.Result);
        };

        yield return checkForUpdateHandle;

        if (catalogsToUpdate.Count > 0)
        {
            print("Updating catalogs..");
            AsyncOperationHandle<List<IResourceLocator>> updateHandle = Addressables.UpdateCatalogs(
                catalogsToUpdate
            );
            yield return updateHandle;
            Addressables.Release(updateHandle);
        }
        else
        {
            print("No catalogs to update");
        }

        if (checkForUpdateHandle.IsValid())
        {
            print("is valid: " + checkForUpdateHandle);
            Addressables.Release(checkForUpdateHandle);
        }
    }

    private IEnumerator LoadAssets()
    {
        yield return new WaitUntil(() => done == labels.Count);
        UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScreen");
    }
}
