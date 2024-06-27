using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Google;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoogleSignInController : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI userName;

    [SerializeField]
    private TextMeshProUGUI statusText;
    [SerializeField]
    private GameObject loggedInScreen, loggedOutScreen;

    private string webClientIdGoogle = "194682062935-ukqi0s2vp1d2nmoembp0dapes21ei859.apps.googleusercontent.com";

    private GoogleSignInConfiguration configuration;

    List<Task> tasks = new List<Task>();

    Task<GoogleSignInUser> taskStatus;

    [SerializeField]
    TitleScreenController titleScreenController;

    int timeoutToCancelInSeconds = 50;
    int timeoutToShowLoadingInSeconds = 5;

    public static GoogleSignInController Instance;

    void Awake()
    {
        configuration = new GoogleSignInConfiguration
        {
            WebClientId = webClientIdGoogle,
            RequestIdToken = true,
            RequestEmail = true,
            RequestProfile = true
        };
    }

    void Start()
    {
        Init();
        AddStatusText("Welcome " + PlayerPrefs.GetString("GoogleUserName"));
        if (GoogleSignIn.Configuration == null)
        {
            GoogleSignIn.Configuration = configuration;
        }
        SignInWithCachedUser();
    }

    void Init()
    {
        // Check if an instance already exists
        if (Instance != null)
        {
            // Destroy the old instance
            Destroy(Instance.gameObject);
        }

        // Set the new instance
        Instance = this;

        // Preserve the new instance between scenes
        DontDestroyOnLoad(gameObject);
    }
    private void SignInWithCachedUser()
    {
        if (PlayerPrefs.GetString("GoogleUserId") != "")
        {
            GoogleSignIn
                .DefaultInstance
                .SignInSilently()
                .ContinueWith(
                    OnAuthenticationFinished,
                    TaskScheduler.FromCurrentSynchronizationContext()
                );
        }
    }

    public async void OnSignInSimple()
    {
        if (GoogleSignIn.Configuration == null)
        {
            GoogleSignIn.Configuration = configuration;
            GoogleSignIn.Configuration.UseGameSignIn = false;
            GoogleSignIn.Configuration.RequestIdToken = true;
        }
        AddStatusText("Calling SignIn");

        TimeSpan timeout = new TimeSpan(0, 0, timeoutToCancelInSeconds);
        TimeSpan loadingTimeout = new TimeSpan(0, 0, timeoutToShowLoadingInSeconds);
        CancellationTokenSource cancellationToken = new CancellationTokenSource();
        CancellationToken token = cancellationToken.Token;
        cancellationToken.CancelAfter(timeout);
        var task = GoogleSignIn.DefaultInstance.SignIn();

        try
        {
            await RunningTask(timeout, loadingTimeout, cancellationToken.Token, task);
        }
        catch (TaskCanceledException e)
        {
            print(e);
            cancellationToken.Cancel();
        }
    }

    public async Task RunningTask(
        TimeSpan timeout,
        TimeSpan loadingTimeout,
        CancellationToken cancellationToken,
        Task<GoogleSignInUser> task
    )
    {
        if (await Task.WhenAny(task, Task.Delay(loadingTimeout, cancellationToken)) != task)
        {
            if (titleScreenController)
            {
                titleScreenController.SetLoadingScreen(true);
            }
        }
        if (await Task.WhenAny(task, Task.Delay(timeout, cancellationToken)) == task)
        {
            if (titleScreenController)
            {
                titleScreenController.SetLoadingScreen(false);
            }
            await task.ContinueWith(
                OnAuthenticationFinished,
                TaskScheduler.FromCurrentSynchronizationContext()
            );
        }
        else
        {
            if (cancellationToken.IsCancellationRequested)
            {
                if (titleScreenController)
                {
                    titleScreenController.SetLoadingScreen(false);
                }
                OnSignOut();
                Errors.Instance.HandleSignInError("Sign in canceled");
                throw new TaskCanceledException();
            }
        }
    }

    public void OnSignOut()
    {
        GoogleSignIn.DefaultInstance.SignOut();
        PlayerPrefs.SetString("GoogleUserName", "");
        PlayerPrefs.SetString("GoogleUserId", "");
        if (loggedInScreen)
        {
            AddStatusText("SingOut");
            userName.text = "Guest";
            loggedInScreen.SetActive(false);
            loggedOutScreen.SetActive(true);
        }
    }

    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        print(task.Status);
        switch (task.Status)
        {
            case TaskStatus.Faulted:
                using (
                    IEnumerator<System.Exception> enumerator = task.Exception
                        .InnerExceptions
                        .GetEnumerator()
                )
                {
                    if (enumerator.MoveNext())
                    {
                        GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)
                            enumerator.Current;
                        print("Got Error: " + error.Status + " " + error.Message);
                        Errors.Instance.HandleSignInError("Developer Error");
                        StartCoroutine(WaitForReload());
                        AddStatusText("Got Error: " + error.Status + " " + error.Message);
                    }
                    else
                    {
                        print("Got Unexpected Exception?!?" + task.Exception);
                    }
                }
                break;
            case TaskStatus.Canceled:
                AddStatusText("Canceled");
                break;
            case TaskStatus.WaitingForActivation:
                AddStatusText("Waiting for activation");
                break;
            case TaskStatus.RanToCompletion:
                StartCoroutine(
                    ServerUtils.GetTokenIdValidation(
                        task.Result.IdToken,
                        rawResponse =>
                        {
                            ServerUtils.TokenResponse response = JsonUtility.FromJson<ServerUtils.TokenResponse>(rawResponse);
                            ServerUtils.SetGatewayToken(response.gateway_jwt);

                            if (PlayerPrefs.GetString("GoogleUserId") == "")
                            {
                                PlayerPrefs.SetString("GoogleUserName", task.Result.DisplayName);
                                PlayerPrefs.SetString("GoogleUserId", task.Result.UserId);
                                PlayerPrefs.SetString("GoogleUserEmail", task.Result.Email);
                                titleScreenController.ChangeToMainscreen();
                            }
                            userName.text = task.Result.DisplayName;
                            loggedOutScreen.SetActive(false);
                            loggedInScreen.SetActive(true);
                            AddStatusText("Activated");
                            AddStatusText(task.Result.DisplayName);
                        },
                        error =>
                        {
                            Errors.Instance.HandleSignInError("SignIn Error");
                            StartCoroutine(WaitForReload());
                            AddStatusText(error);
                        }
                    )
                );
                break;
            default:
                print(task.Status.ToString());
                break;
        }
    }

    IEnumerator WaitForReload()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("TitleScreen");
        Errors.Instance.HideSignInError();
    }

    List<String> messages = new List<String>();

    void AddStatusText(string text)
    {
        if (messages.Count == 5)
        {
            messages.RemoveAt(0);
        }
        messages.Add(text);
        string txt = "";
        foreach (string s in messages)
        {
            txt += "\n" + s;
        }
        statusText.text = txt;
        // statusText.text = text;
    }
}
