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
        Action<string> successCallback = rawResponse => {
            ServerUtils.TokenResponse response = JsonUtility.FromJson<ServerUtils.TokenResponse>(rawResponse);
            ServerUtils.SetGatewayToken(response.gateway_jwt);
            titleScreenController.ChangeToMainscreen();
        };
        Action<string> errorCallback = error => {
            Debug.Log(error);
        };

        if (string.IsNullOrEmpty(ServerUtils.GetGatewayToken()))
        {
            StartCoroutine(ServerUtils.CreateGuestUser(successCallback, errorCallback));
        }
        else
        {
            StartCoroutine(ServerUtils.RefreshToken(successCallback, errorCallback));
        }
    }
}
