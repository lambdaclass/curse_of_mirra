using System.Collections;
using MoreMountains.Tools;
using UnityEngine;

public class MainScreenManager : MonoBehaviour
{
    [SerializeField]
    UIModelManager modelManager;
    string sceneName = "CharacterInfo";
    string characterNameToGo;

    void Start()
    {
        CharactersManager.Instance.SetGoToCharacter(ServerConnection.Instance.selectedCharacterName);
        characterNameToGo = ServerConnection.Instance.selectedCharacterName;
        modelManager.SetModel(characterNameToGo);
    }

    public void GoToCharacteInfo()
    {
        Utils.GoToCharacterInfo(characterNameToGo, sceneName);
    }
}
