using System.Collections;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;

public class MainScreenManager : MonoBehaviour
{
    [SerializeField]
    UIModelManager modelManager;

    [SerializeField]
    TextMeshProUGUI playerNameText;

    void Start()
    {
        modelManager.SetModel(ServerConnection.Instance.selectedCharacterName);
        CharactersManager
            .Instance
            .SetGoToCharacter(ServerConnection.Instance.selectedCharacterName);
        StartCoroutine(GoToCharacterInfo());
        SetPlayerNameUI();
    }

    private void SetPlayerNameUI()
    {
        playerNameText.text =
            PlayerPrefs.GetString("GoogleUserId") != ""
                ? PlayerPrefs.GetString("GoogleUserName")
                : "Guest";
    }

    IEnumerator GoToCharacterInfo()
    {
        yield return new WaitUntil(
            () =>
                modelManager.GetComponentInChildren<ButtonAnimationsMMTouchButton>().executeRelease
                == true
        );
        modelManager.GetComponentInChildren<MMLoadScene>().LoadScene();
    }
}
