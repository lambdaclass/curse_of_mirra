using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GuestSignInController : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI userName;

    [SerializeField]
    private TextMeshProUGUI statusText;
    [SerializeField]
    private GameObject loggedInScreen, loggedOutScreen;

    List<Task> tasks = new List<Task>();

    Task<GoogleSignInUser> taskStatus;

    [SerializeField]
    TitleScreenController titleScreenController;

    int timeoutToCancelInSeconds = 50;
    int timeoutToShowLoadingInSeconds = 5;

    public static GuestSignInController Instance;

    void Awake()
    {
    }

    void Start()
    {
        Init();
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

    public async void SignIn()
    {
        StartCoroutine(
            ServerUtils.CreateGuestUser(
                raw_response =>
                {
                    Debug.Log(raw_response);
                    GuestSignInResponse response = JsonUtility.FromJson<GuestSignInResponse>(raw_response);
                    PlayerPrefs.SetString("gateway_jwt", response.gateway_jwt);
                    PlayerPrefs.SetString("user_id", response.user_id);
                    titleScreenController.ChangeToMainscreen();
                },
                error =>
                {
                    Debug.Log("error");
                    Debug.Log(error);
                }
            )
        );
    }

    private class GuestSignInResponse
    {
        public string user_id;
        public string gateway_jwt;
    }
}
