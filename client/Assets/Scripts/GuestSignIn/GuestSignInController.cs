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
    public static GuestSignInController Instance;

    [SerializeField]
    TitleScreenController titleScreenController;

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
        Action<string> successCallback = raw_response => {
            ServerUtils.TokenResponse response = JsonUtility.FromJson<ServerUtils.TokenResponse>(raw_response);
            PlayerPrefs.SetString("gateway_jwt", response.gateway_jwt);
            PlayerPrefs.SetString("user_id", response.user_id);
            titleScreenController.ChangeToMainscreen();
        };
        Action<string> errorCallback = error => {
            Debug.Log(error);
        };

        if (PlayerPrefs.HasKey("gateway_jwt"))
        {
            StartCoroutine(ServerUtils.RefreshToken(successCallback, errorCallback));
        }
        else
        {
            StartCoroutine(ServerUtils.CreateGuestUser(successCallback, errorCallback));

        }
    }
}
