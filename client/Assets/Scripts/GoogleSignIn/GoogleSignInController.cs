using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Google;
using TMPro;
using UnityEngine;

public class GoogleSignInController : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI userName;

    [SerializeField]
    private TextMeshProUGUI statusText;

    [SerializeField]
    private GameObject signOutButton;

    [SerializeField]
    private GameObject signInButton;
    private string webClientIdGoogle = "194682062935-ukqi0s2vp1d2nmoembp0dapes21ei859.apps.googleusercontent.com";

    private GoogleSignInConfiguration configuration;

    List<Task> tasks = new List<Task>();

    Task<GoogleSignInUser> taskStatus;

    [SerializeField]
    TitleScreenController titleScreenController;

    int timeoutToCancelInSeconds = 50;
    int timeoutToShowLoadingInSeconds = 5;

    public static GoogleSignInController Instance;

    private static bool isTrue;

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
            titleScreenController.SetLoadingScreen(true);
        }
        if (await Task.WhenAny(task, Task.Delay(timeout, cancellationToken)) == task)
        {
            titleScreenController.SetLoadingScreen(false);
            await task.ContinueWith(
                OnAuthenticationFinished,
                TaskScheduler.FromCurrentSynchronizationContext()
            );
        }
        else
        {
            if (cancellationToken.IsCancellationRequested)
            {
                titleScreenController.SetLoadingScreen(false);
                OnSignOut();
                Errors.Instance.HandleSignInError();
                throw new TaskCanceledException();
            }
        }
    }

    public void OnSignOut()
    {
        GoogleSignIn.DefaultInstance.SignOut();
        AddStatusText("SingOut");
        PlayerPrefs.SetString("GoogleUserName", "");
        PlayerPrefs.SetString("GoogleUserId", "");
        userName.text = "Guest";
        signOutButton.SetActive(false);
        signInButton.SetActive(true);
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
                        response =>
                        {
                            if (PlayerPrefs.GetString("GoogleUserId") == "")
                            {
                                PlayerPrefs.SetString("GoogleUserName", task.Result.DisplayName);
                                PlayerPrefs.SetString("GoogleUserId", task.Result.UserId);
                            }
                            userName.text = task.Result.DisplayName;
                            signInButton.SetActive(false);
                            signOutButton.SetActive(true);
                            AddStatusText("Activated");
                            AddStatusText(task.Result.DisplayName);
                        },
                        error =>
                        {
                            print(error);
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



