using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class TitleScreenController : MonoBehaviour
{
    private const string TITLE_SCENE_NAME = "MainScreen";
    [SerializeField]
    CanvasGroup playNowButton;

    [SerializeField]
    Image logoImage;

    [SerializeField]
    PlayerNameHandler playerNameHandler;

    [SerializeField]
    CanvasGroup changeNameButton;

    [SerializeField]
    GameObject playerNamePopUp;

    [SerializeField]
    GameObject loadingScreen;
    [SerializeField]
    Image loadingSpinner;
    private AsyncOperation asyncOperation;

    void Start()
    {
        StartCoroutine(FadeIn(logoImage.GetComponent<CanvasGroup>(), 1f, .1f));
        StartCoroutine(FadeIn(playNowButton, .3f, 1.2f));
        StartCoroutine(FadeIn(changeNameButton, 1f, 1.2f));
        if (PlayerPrefs.GetString("playerName") == "")
        {
            playerNamePopUp.SetActive(true);
            StartCoroutine(FadeIn(playerNamePopUp.GetComponent<CanvasGroup>(), 1f, 1.2f));
        }
        if (this.asyncOperation == null)
        {
            this.StartCoroutine(this.LoadSceneAsyncProcess(TITLE_SCENE_NAME));
        }
    }

    private IEnumerator LoadSceneAsyncProcess(string sceneName)
    {
        asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;
        while (asyncOperation.isDone)
        {
            yield return null;
        }
    }

    public void PlayButton()
    {
        StartCoroutine(ChangeToMainScreen());
    }

    private IEnumerator ChangeToMainScreen() {
        SetLoadingScreen(true);
        yield return StartCoroutine(Utils.GetSelectedCharacter());
        if(asyncOperation != null)
        {
            asyncOperation.allowSceneActivation = true;
        }
    }

    IEnumerator FadeIn(CanvasGroup element, float time, float delay)
    {
        yield return new WaitForSeconds(delay);

        for (float i = 0; i <= 1; i += Time.deltaTime / time)
        {
            element.alpha = i;
            yield return null;
        }
    }

    void SetLoadingScreen(bool isActive)
    {
        loadingScreen.SetActive(isActive);
        if(isActive)
        {
            loadingSpinner.transform.DORotate(new Vector3(0, 0, -360), .5f, RotateMode.Fast)
                .SetLoops(-1, LoopType.Restart)
                .SetRelative()
                .SetEase(Ease.InOutQuad);
        }
    }

    public void ShowPlayerNamePopUp()
    {
        this.playerNameHandler.Show();
    }
}
