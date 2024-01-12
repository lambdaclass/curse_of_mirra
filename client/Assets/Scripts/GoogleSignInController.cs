using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google;
using System.Threading.Tasks;
using TMPro;
using System;
using System.Threading;
using System.Diagnostics;

public class GoogleSignInController : MonoBehaviour
{
    public TextMeshProUGUI statusText;

    [SerializeField]
    private GameObject signOutButton;

    [SerializeField]
    private GameObject signInButton;
    public string webClientIdGoogle =
        "529212382177-822ukg0eeufi7pivtk1dpatqveqlqord.apps.googleusercontent.com";

    private GoogleSignInConfiguration configuration;

    List<Task> tasks = new List<Task>();

    Task<GoogleSignInUser> taskStatus;

    // Can be set via the property inspector in the Editor.AA
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
            GoogleSignIn.DefaultInstance
                .SignInSilently()
                .ContinueWith(
                    OnAuthenticationFinished,
                    TaskScheduler.FromCurrentSynchronizationContext()
                );
            print("Esta logeado ya");
        }
    }

    public async void OnSignIn()
    {
        print(configuration);
        print(GoogleSignIn.Configuration);
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        AddStatusText("Calling SignIn");

        TimeSpan timeout = new TimeSpan(0, 0, 30);
        CancellationTokenSource cancellationToken = new CancellationTokenSource();
        CancellationToken token = cancellationToken.Token;
        cancellationToken.CancelAfter(timeout);
        var task = GoogleSignIn.DefaultInstance.SignIn();

        //  Task cancellation method 1

        try
        {
            await RunningTask(timeout, cancellationToken.Token, task);
        }
        catch (TaskCanceledException e)
        {
            print(e);
            cancellationToken.Cancel();
        }

        //Cancelation method 2

        // if (await Task.WhenAny(task, Task.Delay(timeout, cancellationToken.Token)) == task)
        // {
        //     print("Entro al task");
        //     await task.ContinueWith(
        //         OnAuthenticationFinished,
        //         TaskScheduler.FromCurrentSynchronizationContext()
        //     );
        // }
        // else
        // {
        //     token.ThrowIfCancellationRequested();
        //     cancellationToken.Cancel();
        //     print("Cancel request " + cancellationToken.IsCancellationRequested);
        //     // print("Cancel");
        //     print(task.Status);
        //     print(cancellationToken.Token);
        // }
    }

    public async Task RunningTask(
        TimeSpan timeout,
        CancellationToken cancellationToken,
        Task<GoogleSignInUser> task
    )
    {
        if (await Task.WhenAny(task, Task.Delay(timeout, cancellationToken)) == task)
        {
            print("Entro al task");
            await task.ContinueWith(
                OnAuthenticationFinished,
                TaskScheduler.FromCurrentSynchronizationContext()
            );
        }
        else
        {
            if (cancellationToken.IsCancellationRequested)
            {
                throw new TaskCanceledException();
            }
            // cancelat.ThrowIfCancellationRequested();
            // cancellationToken.Cancel();
            // print("Cancel request " + cancellationToken.IsCancellationRequested);
            // // print("Cancel");
            // print(task.Status);
            // print(cancellationToken.Token);
        }
    }

    public void OnSingOut()
    {
        // print(taskStatus.Status);
        // if (taskStatus.Status == TaskStatus.RanToCompletion)
        // {
        GoogleSignIn.DefaultInstance.SignOut();
        AddStatusText("SingOut");
        PlayerPrefs.SetString("GoogleUserName", "");
        PlayerPrefs.SetString("GoogleUserId", "");
        PlayerPrefs.SetString("GoogleIdToke", "");
        signOutButton.SetActive(false);
        signInButton.SetActive(true);
        // }
    }

    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        print(task.Status);
        switch (task.Status)
        {
            case TaskStatus.Faulted:
                using (
                    IEnumerator<System.Exception> enumerator =
                        task.Exception.InnerExceptions.GetEnumerator()
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
                signInButton.SetActive(false);
                signOutButton.SetActive(true);
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
