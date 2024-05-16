using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScreenSettings : MonoBehaviour
{
    [SerializeField] private GameObject DisconnectButton;
    [SerializeField] private GameObject ConnectButton;

    void Start()
    {
        if (PlayerPrefs.GetString("GoogleUserId") == "")
        {
            ConnectAccount();
        }
        else
        {
            DisconnectAccount();
        }
    }


    private void DisconnectAccount()
    {
        DisconnectButton.SetActive(true);
        ConnectButton.SetActive(false);

    }

    private void ConnectAccount()
    {
        ConnectButton.SetActive(true);
        DisconnectButton.SetActive(false);
    }

    public void DisconnectFromAccount()
    {
        GoogleSignInController.Instance.OnSignOut();
        GoToTitleScreen();
    }
    private void GoToTitleScreen()
    {
        SceneManager.LoadScene("TitleScreen");
    }

}
