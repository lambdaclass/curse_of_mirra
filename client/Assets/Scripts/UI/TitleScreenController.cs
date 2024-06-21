using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenController : MonoBehaviour
{
    private const string TITLE_SCENE_NAME = "DailyRewards";

    [SerializeField]
    CanvasGroup ButtonsCanvas;

    [SerializeField]
    MMTouchButton playNowButton;

    [SerializeField]
    Image logoImage;

    [SerializeField]
    PlayerNameHandler playerNameHandler;

    [SerializeField]
    CanvasGroup changeNameButton;

    [SerializeField] CanvasGroup SingOutContainer, SingInContainer;

    [SerializeField]
    GameObject playerNamePopUp;

    [SerializeField]
    GameObject loadingScreen;

    [SerializeField] TextMeshProUGUI userText;

    [SerializeField]
    Image loadingSpinner;
    private AsyncOperation asyncOperation;
    private Tween spinnerRotationTween;
    private List<String> avaibleCharactersNames = new List<String>();

    private string selectedCharacterName;
    void Start()
    {
        avaibleCharactersNames = CharactersManager.Instance.GetAvailableCharactersNames();
        StartCoroutine(FadeIn(logoImage.GetComponent<CanvasGroup>(), 1f, .1f));
        StartCoroutine(FadeIn(ButtonsCanvas, .3f, 1.2f));
        StartCoroutine(FadeIn(changeNameButton, 1f, 1.2f));
        StartCoroutine(FadeIn(SingOutContainer, 1f, 1.2f));
        StartCoroutine(FadeIn(SingInContainer, .3f, 1.2f));

        if (PlayerPrefs.GetString("GoogleUserId") == "")
        {
            SingOutContainer.gameObject.SetActive(true);
            SingInContainer.gameObject.SetActive(false);
            userText.text = "Guest";
        }
        else
        {
            SingInContainer.gameObject.SetActive(true);
            SingOutContainer.gameObject.SetActive(false);
            userText.text = PlayerPrefs.GetString("GoogleUserName");
        }

        if (this.asyncOperation == null)
        {
            this.StartCoroutine(this.LoadSceneAsyncProcess(TITLE_SCENE_NAME));
        }

        selectedCharacterName = PlayerPrefs.GetString("selectedCharacterName") == ""
            ? "Muflus"
            : PlayerPrefs.GetString("selectedCharacterName");
    }

    private IEnumerator LoadSceneAsyncProcess(string sceneName)
    {
        asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;
        while (
            !asyncOperation.isDone
            && !String.IsNullOrEmpty(ServerConnection.Instance.selectedCharacterName)
        )
        {
            yield return null;
        }
    }

    public void ChangeToMainscreen()
    {
        SetLoadingScreen(true);
        // SelectCharacterAndMaybeCreateUser();
        ServerConnection.Instance.selectedCharacterName = selectedCharacterName;
        asyncOperation.allowSceneActivation = true;
    }

    public void SelectCharacterAndMaybeCreateUser()
    {
        StartCoroutine(
            ServerUtils.GetSelectedCharacter(
                response =>
                {
                    if (!avaibleCharactersNames.Contains(response.selected_character))
                    {
                        // If the character selected is currently not available
                        // Selects the first avaible character in the list and notice the user
                        Errors
                            .Instance
                            .HandleNetworkError(
                                "Attention!",
                                response.selected_character
                                    + " is currently unavailable "
                                    + "\n"
                                    + avaibleCharactersNames[0]
                                    + " has been selected"
                            );
                        StartCoroutine(
                            ServerUtils.SetSelectedCharacter(
                                avaibleCharactersNames[0],
                                response =>
                                {
                                    ServerConnection.Instance.selectedCharacterName =
                                        response.selected_character;
                                    asyncOperation.allowSceneActivation = true;
                                },
                                erorr =>
                                {
                                    ErrorHandler("Oops!", "Something went wrong");
                                }
                            )
                        );
                    }
                    else
                    {
                        if (asyncOperation != null)
                        {
                            ServerConnection.Instance.selectedCharacterName =
                                response.selected_character;
                            asyncOperation.allowSceneActivation = true;
                        }
                    }
                },
                error =>
                {
                    switch (error)
                    {
                        case "USER_NOT_FOUND":
                            CreateUser();
                            break;
                        case "CONNECTION_ERROR":
                            ErrorHandler("Oops!", "No Server Available to Connect");
                            break;
                        default:
                            ErrorHandler("Oops!", error);
                            break;
                    }
                }
            )
        );
    }

    private void ErrorHandler(string errorTitle, string errorMessage)
    {
        Errors.Instance.HandleNetworkError(errorTitle, errorMessage);
        SetLoadingScreen(false);
        playNowButton.EnableButton();
    }

    private void CreateUser()
    {
        StartCoroutine(
            ServerUtils.CreateUser(
                response =>
                {
                    ServerConnection.Instance.GetSelectedCharacter(asyncOperation);
                },
                error =>
                {
                    switch (error)
                    {
                        case "USER_ALREADY_TAKEN":
                            Errors.Instance.HandleNetworkError("Error", "ClientId already taken");
                            break;
                        default:
                            Errors.Instance.HandleNetworkError("Error", error);
                            break;
                    }
                    SetLoadingScreen(false);
                    playNowButton.EnableButton();
                }
            )
        );
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

    public void SetLoadingScreen(bool isActive)
    {
        loadingScreen.SetActive(isActive);
        if (isActive)
        {
            spinnerRotationTween = loadingSpinner
                .transform
                .DORotate(new Vector3(0, 0, -360), .5f, RotateMode.Fast)
                .SetLoops(-1, LoopType.Restart)
                .SetRelative()
                .SetEase(Ease.InOutQuad);
        }
        else
        {
            spinnerRotationTween.Kill();
        }
    }

    public void ShowPlayerNamePopUp()
    {
        this.playerNameHandler.Show();
    }
}
