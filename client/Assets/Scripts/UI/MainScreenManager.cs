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
    string sceneName = "CharacterInfo";
    string characterNameToGo;

    void Start()
    {
        CharactersManager
            .Instance
            .SetGoToCharacter(ServerConnection.Instance.selectedCharacterName);
        characterNameToGo = ServerConnection.Instance.selectedCharacterName;
        modelManager.SetModel(characterNameToGo);
        playerName.text = PlayerPrefs.GetString("playerName");
    }

    public void GoToCharacteInfo()
    {
        Utils.GoToCharacterInfo(characterNameToGo, sceneName);
    }
}
