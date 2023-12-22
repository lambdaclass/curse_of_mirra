using System.Collections;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;

public class MainScreenManager : MonoBehaviour
{
    [SerializeField]
    UIModelManager modelManager;

    [SerializeField]
    TextMeshProUGUI playerName;

    void Start()
    {
        modelManager.SetModel(LobbyConnection.Instance.selectedCharacterName);
        CharactersManager.Instance.SetGoToCharacter(LobbyConnection.Instance.selectedCharacterName);
        StartCoroutine(GoToCharacterInfo());

        playerName.text = LobbyConnection.Instance.username;
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
