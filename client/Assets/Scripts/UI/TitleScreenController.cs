using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System;

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
        ChangeToMainScreen();
    }

    private void ChangeToMainScreen() {
        SetLoadingScreen(true);
        StartCoroutine(Utils.GetSelectedCharacter(
            response => {
                GameManager.Instance.selectedCharacterName = response.selected_character;
                if(asyncOperation != null)
                {
                    asyncOperation.allowSceneActivation = true;
                }
            },
            error => {
                switch(error) {
                    case "NOT_FOUND":
                        CreateUser();
                        break;
                    case "CONNECTION_ERROR":
                        // The Errors gameObject is not in titleScreen scene yet
                        // Errors.Instance.HandleNetworkError("Error", error);
                        break;
                }
            }
        ));
    }

    private void CreateUser() {
        StartCoroutine(Utils.CreateUser(
            response => {
                GameManager.Instance.selectedCharacterName = response.selected_character;
                print("User created");
                if(asyncOperation != null)
                {
                    asyncOperation.allowSceneActivation = true;
                }
            },
            error => {
                switch(error) {
                    case "USER_ALREADY_TAKEN":
                        Debug.LogError("clientId already taken");
                        break;
                    case "CONNECTION_ERROR":
                        // The Errors gameObject is not in titleScreen scene yet
                        // Errors.Instance.HandleNetworkError("Error", error);
                        break;
                }
            }
        ));
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
