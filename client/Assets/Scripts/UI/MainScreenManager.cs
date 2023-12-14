using UnityEngine;

public class MainScreenManager : MonoBehaviour
{
    [SerializeField]
    UIModelManager modelManager;

    [SerializeField]
    GameObject lobbyConnectionPrefab;

    void Start()
    {
        modelManager.SetModel();
    }
}
