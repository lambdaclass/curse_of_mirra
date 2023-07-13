using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreloadSceneInUnity : MonoBehaviour
{
    [SerializeField]
    private string sceneName = "BackendPlayground";
    public string SceneName => this.sceneName;

    private AsyncOperation asyncOperation;

    private IEnumerator LoadSceneAsyncProcess(string sceneName)
    {
        // Begin to load the Scene you have specified.
        this.asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        // Don't let the Scene activate until you allow it to.
        this.asyncOperation.allowSceneActivation = false;

        while (!this.asyncOperation.isDone)
        {
            Debug.Log($"[scene]:{sceneName} [load progress]: {this.asyncOperation.progress}");

            yield return null;
        }
    }

    private void Start()
    {
        if (this.asyncOperation == null)
        {
            this.StartCoroutine(this.LoadSceneAsyncProcess(sceneName: this.sceneName));
        }
    }

    private void Update()
    {
        AllowSceneActivation();
    }

    public void AllowSceneActivation()
    {
        if (SocketConnectionManager.Instance.allSelected && this.asyncOperation != null)
        {
            this.asyncOperation.allowSceneActivation = true;
        }
    }
}
