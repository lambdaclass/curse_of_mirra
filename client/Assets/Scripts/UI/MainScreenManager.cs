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
        characterNameToGo = ServerConnection.Instance.selectedCharacterName;
        modelManager.SetModel(characterNameToGo);
    }

    public void GoToCharacteInfo()
    {
        Utils.GoToCharacterInfo(characterNameToGo, sceneName);
    }
}
