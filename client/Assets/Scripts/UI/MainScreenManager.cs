using System.Collections;
using MoreMountains.Tools;
using UnityEngine;

public class MainScreenManager : MonoBehaviour
{
    [SerializeField]
    UIModelManager modelManager;

    void Start()
    {
        string characterSelectedName = ServerConnection.Instance.selectedCharacterName;
        modelManager.SetModel(characterSelectedName);
        GetComponent<GoToCharacterInfo>().characterNameString = characterSelectedName;
    }
}
