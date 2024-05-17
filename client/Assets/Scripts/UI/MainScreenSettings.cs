using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScreenSettings : MonoBehaviour
{
    [SerializeField] private GameObject DisconnectButton;
    [SerializeField] private GameObject ConnectButton;
    [SerializeField] private GameObject AccountMail, AccountLabel;

    void Start()
    {
        if (PlayerPrefs.GetString("GoogleUserId") == "")
        {
            ConnectAccount();
        }
        else
        {
            AccountLabel.SetActive(true);
            AccountMail.SetActive(true);
            AccountMail.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("GoogleUserEmail");
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

    public void DisconnectFromGoogleAccount()
    {
        GoogleSignInController.Instance.OnSignOut();
        AccountLabel.SetActive(false);
        AccountMail.SetActive(false);
        ConnectAccount();
    }

    public void ConnectToGoogleAccount()
    {
        GoogleSignInController.Instance.OnSignInSimple();
        StartCoroutine(WaitForGoogleResponse());
    }

    IEnumerator WaitForGoogleResponse()
    {
        yield return new WaitUntil(() => PlayerPrefs.GetString("GoogleUserId") != "");
        AccountLabel.SetActive(true);
        AccountMail.SetActive(true);
        AccountMail.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("GoogleUserEmail");
        DisconnectAccount();
    }
}
