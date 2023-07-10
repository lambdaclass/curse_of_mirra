using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterSelectionManager : MonoBehaviour
{
    [SerializeField]
    CharacterSelectionList playersList;

    [SerializeField]
    CharacterSelectionUI characterList;
    public bool selected = false;

    void Start()
    {
        playersList.CreatePlayerItems();
    }

    void Update()
    {
        // if (
        //     selected == false
        //     && playersList.GetPlayerCharacter(LobbyConnection.Instance.playerId) != null
        // )
        // {
        //     selected = true;
        //     print("Created item for" + LobbyConnection.Instance.playerId);
        //     playersList.CreatePlayerItem(LobbyConnection.Instance.playerId);
        // }

        if (characterList.updated == true)
        {
            characterList.updated = false;
            UICharacterItem updatedCharacter = GetSelectedCharacter();
            print("Updated character for" + LobbyConnection.Instance.playerId);
            playersList.UpdatePlayerItem(
                LobbyConnection.Instance.playerId,
                updatedCharacter.name.text
            );
        }

        // if (
        //     selected
        //     && playersList.playerItems.Count
        //         > SocketConnectionManager.Instance.selectedCharacters?.Count
        // )
        // {
        //     playersList.removePlayerItems();
        // }

        // playersList.DisplayPlayerItems();
        playersList.DisplayUpdates();
    }

    public UICharacterItem GetSelectedCharacter()
    {
        List<GameObject> allCharacter = characterList.GetAllChilds();
        UICharacterItem selectedCharacter = allCharacter
            ?.Find(el => el.GetComponent<UICharacterItem>().selected == true)
            .GetComponent<UICharacterItem>();
        return selectedCharacter != null ? selectedCharacter : null;
    }
}
