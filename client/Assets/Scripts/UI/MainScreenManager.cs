using UnityEngine;

public class MainScreenManager : MonoBehaviour
{
    [SerializeField]
    UIModelManager modelManager;

    void Start()
    {
        modelManager.SetModel(CharactersManager.Instance.selectedCharacterName);
    }
}
