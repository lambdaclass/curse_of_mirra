using System.Collections;
using MoreMountains.Tools;
using UnityEngine;

public class MainScreenManager : MonoBehaviour
{
    [SerializeField]
    UIModelManager modelManager;

    void Start()
    {
        modelManager.SetModel(LobbyConnection.Instance.selectedCharacterName);
        CharactersManager.Instance.SetGoToCharacter(LobbyConnection.Instance.selectedCharacterName);
        StartCoroutine(GoToCharacterInfo());
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
