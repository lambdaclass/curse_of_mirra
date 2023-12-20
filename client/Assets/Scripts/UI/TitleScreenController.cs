using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using MoreMountains.Tools;
using System.Collections.Generic;
using System.Linq;
using System;

public class TitleScreenController : MonoBehaviour
{
    private const string TITLE_SCENE_NAME = "MainScreen";

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

    [SerializeField]
    GameObject playerNamePopUp;

    [SerializeField]
    GameObject loadingScreen;

    [SerializeField]
    Image loadingSpinner;
    private AsyncOperation asyncOperation;
    private Tween spinnerRotationTween;
    private List<String> avaibleCharactersNames = new List<String>();

    void Start()
    {
        avaibleCharactersNames = CharactersManager.Instance.GetAvailableCharactersNames();
        StartCoroutine(FadeIn(logoImage.GetComponent<CanvasGroup>(), 1f, .1f));
        StartCoroutine(FadeIn(ButtonsCanvas, .3f, 1.2f));
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
        while (
            !asyncOperation.isDone
            && !String.IsNullOrEmpty(LobbyConnection.Instance.selectedCharacterName)
        )
        {
            yield return null;
        }
    }

    public void ChangeToMainscreen()
    {
        SetLoadingScreen(true);
        SelectCharacterAndMaybeCreateUser();
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
                        Errors.Instance.HandleNetworkError(
                            "Atention!",
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
                                    LobbyConnection.Instance.selectedCharacterName =
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
                            LobbyConnection.Instance.selectedCharacterName =
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
                            ErrorHandler("Oops!", "No Server Avaible to Connect");
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
                    if (asyncOperation != null)
                    {
                        asyncOperation.allowSceneActivation = true;
                    }
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

    void SetLoadingScreen(bool isActive)
    {
        loadingScreen.SetActive(isActive);
        if (isActive)
        {
            spinnerRotationTween = loadingSpinner.transform
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
