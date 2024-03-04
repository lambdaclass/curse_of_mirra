using System.Collections;
using MoreMountains.Tools;
using UnityEngine;

public class MainScreenManager : MonoBehaviour
{
    [SerializeField]
    UIModelManager modelManager;

    void Start()
    {
        modelManager.SetModel(ServerConnection.Instance.selectedCharacterName);
        CharactersManager
            .Instance
            .SetGoToCharacter(ServerConnection.Instance.selectedCharacterName);
    }

    public void GoToCharacterInfo()
    {
        modelManager.GetComponentInChildren<MMLoadScene>().LoadScene();
    }
}
