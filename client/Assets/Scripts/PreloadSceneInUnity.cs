using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreloadSceneInUnity : MonoBehaviour
{
    [SerializeField]
    private string _sceneName = "maingame";
    public string _SceneName => this._sceneName;

    private AsyncOperation _asyncOperation;

    private IEnumerator LoadSceneAsyncProcess(string sceneName)
    {
        // Begin to load the Scene you have specified.
        this._asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        // Don't let the Scene activate until you allow it to.
        this._asyncOperation.allowSceneActivation = false;

        while (!this._asyncOperation.isDone)
        {
            Debug.Log($"[scene]:{sceneName} [load progress]: {this._asyncOperation.progress}");

            yield return null;
        }
    }

    private void Start()
    {
        if (this._asyncOperation == null)
        {
            this.StartCoroutine(this.LoadSceneAsyncProcess(sceneName: this._sceneName));
        }
    }

    private void Update()
    {
        AllowSceneActivation();
    }

    public void AllowSceneActivation()
    {
        if (SocketConnectionManager.Instance.allSelected && this._asyncOperation != null)
        {
            this._asyncOperation.allowSceneActivation = true;
        }
    }
}
