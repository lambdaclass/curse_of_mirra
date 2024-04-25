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
    public TextMeshProUGUI statusText;

    // [SerializeField]
    // private GameObject signOutButton;

    [SerializeField]
    private GameObject signInButton;
    public string webClientIdGoogle =
        "529212382177-822ukg0eeufi7pivtk1dpatqveqlqord.apps.googleusercontent.com";

    private GoogleSignInConfiguration configuration;

    List<Task> tasks = new List<Task>();

    Task<GoogleSignInUser> taskStatus;

    [SerializeField]
    TitleScreenController titleScreenController;

    int timeoutToCancelInSeconds = 50;
    int timeoutToShowLoadingInSeconds = 5;

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
        AddStatusText("Welcome " + PlayerPrefs.GetString("GoogleUserName"));
        SignInWithCachedUser();
    }

    private void SignInWithCachedUser()
    {
        if (PlayerPrefs.GetString("GoogleUserId") != "")
        {
            GoogleSignIn.Configuration = configuration;
            GoogleSignIn
                .DefaultInstance
                .SignInSilently()
                .ContinueWith(
                    OnAuthenticationFinished,
                    TaskScheduler.FromCurrentSynchronizationContext()
                );
            print("Esta logeado ya");
        }
    }

    public async void OnSignInSimple()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
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
            print("Entro al task");
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
        PlayerPrefs.SetString("GoogleIdToke", "");
        // signOutButton.SetActive(false);
        // signInButton.SetActive(true);
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
                        AddStatusText("Got Error: " + error.Status + " " + error.Message);
                    }
                    else
                    {
                        AddStatusText("Got Unexpected Exception?!?" + task.Exception);
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
                if (PlayerPrefs.GetString("GoogleUserId") == "")
                {
                    PlayerPrefs.SetString("GoogleUserName", task.Result.DisplayName);
                    PlayerPrefs.SetString("GoogleUserId", task.Result.UserId);
                    PlayerPrefs.SetString("GoogleIdToke", task.Result.IdToken);
                }
                // signInButton.SetActive(false);
                // signOutButton.SetActive(true);
                AddStatusText("Welcome: " + task.Result.DisplayName + "!");
                break;
            default:
                AddStatusText(task.Status.ToString());
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
