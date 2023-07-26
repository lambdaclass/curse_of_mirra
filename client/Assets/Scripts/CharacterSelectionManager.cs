using UnityEngine;

public class CharacterSelectionManager : MonoBehaviour
{
    [SerializeField]
    CharacterSelectionList playersList;

    void Start()
    {
        playersList.CreatePlayerItems();
    }

    void Update()
    {
        playersList.DisplayUpdates();
    }
}
