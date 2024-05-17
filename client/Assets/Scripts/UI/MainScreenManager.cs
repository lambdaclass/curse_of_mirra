using System;
using System.Collections;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainScreenManager : MonoBehaviour
{
    private const string BATTLE_SCENE_NAME = "Battle";
    private const string LOBBY_SCENE_NAME = "Lobby";
    private const string MAIN_SCENE_NAME = "MainScreen";
    private const string CHARACTER_INFO_SCENE_NAME = "CharacterInfo";
    private const string TITLE_SCENE_NAME = "TitleScreen";

    [SerializeField]
    UIModelManager modelManager;

    [SerializeField]
    TextMeshProUGUI playerName;

    [SerializeField]
    TextMeshProUGUI currentPlayerName;

    string characterNameToGo;
    [SerializeField] GameObject playerNamePopUp;


    void Start()
    {
        CharactersManager
            .Instance
            .SetGoToCharacter(ServerConnection.Instance.selectedCharacterName);
        characterNameToGo = ServerConnection.Instance.selectedCharacterName;
        modelManager.SetModel(characterNameToGo);
        playerName.text = PlayerPrefs.GetString("playerName");
        if (PlayerPrefs.GetString("playerName") == "")
        {
            ShowPlayerNamePopUp();
        }
        currentPlayerName.text = "Current name: " + PlayerPrefs.GetString("playerName");
    }

    public void GoToCharacteInfo()
    {
        Utils.GoToCharacterInfo(characterNameToGo, CHARACTER_INFO_SCENE_NAME);
    }

    public void ShowPlayerNamePopUp()
    {
        playerNamePopUp.SetActive(true);
        StartCoroutine(FadeIn(playerNamePopUp.GetComponent<CanvasGroup>(), 0.6f, 0f));
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

}
