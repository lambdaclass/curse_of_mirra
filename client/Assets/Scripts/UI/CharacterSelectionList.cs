using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterSelectionList : MonoBehaviour
{
    [SerializeField]
    GameObject playerItemPrefab;

    [SerializeField]
    CharacterSelectionUI characterItems;
    public List<GameObject> playerItems = new List<GameObject>();

    public void CreatePlayerItems()
    {
        for (int i = 0; i < LobbyConnection.Instance.playerCount; i++)
        {
            CreatePlayerItem((ulong)i + 1);
        }
    }

    public void DisplayUpdates()
    {
        Dictionary<ulong, string> selectedCharacters = SocketConnectionManager
            .Instance
            .selectedCharacters;

        if (selectedCharacters?.Count > 0 && playerItems.Count > 0)
        {
            foreach (KeyValuePair<ulong, string> entry in selectedCharacters)
            {
                if (GetUpdatedItem(entry.Key, entry.Value))
                {
                    UpdatePlayerItem(entry.Key, entry.Value);
                }
            }
        }
    }

    public bool GetUpdatedItem(ulong key, string value)
    {
        return playerItems.Find(
            el =>
                el.GetComponent<CharacterSelectionPlayerItem>().GetId() == key
                && el.GetComponent<CharacterSelectionPlayerItem>().GetName() != value
        );
    }

    public void CreatePlayerItem(ulong id)
    {
        GameObject newPlayer = Instantiate(playerItemPrefab, gameObject.transform);
        CharacterSelectionPlayerItem playerI =
            newPlayer.GetComponent<CharacterSelectionPlayerItem>();
        playerI.SetId(id);
        playerI.SetCharacterName("Not Selected");
        playerI.SetPlayerItemText();

        playerItems.Add(newPlayer);
    }

    public void UpdatePlayerItem(ulong id, string character)
    {
        if (playerItems.Count > 0)
        {
            CharacterSelectionPlayerItem playerI = playerItems
                ?.Find(el => el.GetComponent<CharacterSelectionPlayerItem>().GetId() == id)
                ?.GetComponent<CharacterSelectionPlayerItem>();
            playerI.SetCharacterName(character);
            playerI.SetPlayerItemText();
            CoMCharacter ui = characterItems.GetSelectedCharacter(character);
            playerI.SetSprite(ui.selectedArtwork);
        }
    }
}
