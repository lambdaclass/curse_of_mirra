using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenController : MonoBehaviour
{
    private const string TITLE_SCENE_NAME = "MainScreen";
    public string clientId;

    [SerializeField]
    CanvasGroup playNowButton;

    [SerializeField]
    Image logoImage;

    [SerializeField]
    PlayerNameHandler playerNameHandler;

    [SerializeField]
    CanvasGroup changeNameButton;

    [SerializeField]
    GameObject playerNamePopUp;
    class AcceptAllCertificates : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }

    void Start()
    {
        LoadClientId();
        StartCoroutine(FadeIn(logoImage.GetComponent<CanvasGroup>(), 1f, .1f));
        StartCoroutine(FadeIn(playNowButton, .3f, 1.2f));
        StartCoroutine(FadeIn(changeNameButton, 1f, 1.2f));
        if (PlayerPrefs.GetString("playerName") == "")
        {
            playerNamePopUp.SetActive(true);
            StartCoroutine(FadeIn(playerNamePopUp.GetComponent<CanvasGroup>(), 1f, 1.2f));
        }
    }

    public void PlayButton()
    {
        StartCoroutine(FetchUserData());
        // loading screen HERE
        SceneManager.LoadScene(TITLE_SCENE_NAME);
    }

    private IEnumerator ChangeToMainScreen() {
        yield return FetchUserData();
    }

    IEnumerator FadeIn(CanvasGroup element, float time, float delay)
    {
        yield return new WaitForSeconds(delay);

        for (float i = 0; i <= 1; i += Time.deltaTime / time)
        {
            element.alpha = i;
            yield return null;
        }
    }

    public void ShowPlayerNamePopUp()
    {
        this.playerNameHandler.Show();
    }

    public IEnumerator FetchUserData()
    {
        string url = makeUrl("/users-characters/" + this.clientId);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.certificateHandler = new AcceptAllCertificates();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    if(webRequest.downloadHandler.text.Contains("INEXISTENT_USER")) {
                        Errors.Instance.HandleNetworkError("Error", webRequest.downloadHandler.text);
                    } else {
                        UserCharacterResponse response = JsonUtility.FromJson<UserCharacterResponse>(
                            webRequest.downloadHandler.text
                        );
                        PlayerPrefs.SetString("selected_character", response.selected_character);
                    }
                    break;
                default:
                    Errors.Instance.HandleNetworkError("Error", webRequest.downloadHandler.error);
                    break;
            }
        }
        yield return null;
    }

    // This code is duplicated from LobbyConnection.cs
    private string makeUrl(string path)
    {
        if (SelectServerIP.GetServerIp().Contains("localhost"))
        {
            return "http://" + SelectServerIP.GetServerIp() + ":4000" + path;
        }
        else if (SelectServerIP.GetServerIp().Contains("10.150.20.186"))
        {
            return "http://" + SelectServerIP.GetServerIp() + ":4000" + path;
        }
        else
        {
            return "https://" + SelectServerIP.GetServerIp() + path;
        }
    }

    private void LoadClientId()
    {
        if (!PlayerPrefs.HasKey("client_id"))
        {
            Guid g = Guid.NewGuid();
            PlayerPrefs.SetString("client_id", g.ToString());
        }

        this.clientId = PlayerPrefs.GetString("client_id");
    }
}
