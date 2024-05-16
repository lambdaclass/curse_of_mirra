using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScreenSettings : MonoBehaviour
{
    [SerializeField] private GameObject DisconnectButton;
    [SerializeField] private GameObject ConnectButton;
    [SerializeField] private GameObject AccountText;

    void Start()
    {
        if (PlayerPrefs.GetString("GoogleUserId") == "")
        {
            ConnectAccount();
        }
        else
        {
            AccountText.SetActive(true);
            AccountText.GetComponent<TextMeshProUGUI>().text += " " + PlayerPrefs.GetString("GoogleUserEmail");
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
