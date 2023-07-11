using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterSelectionList : MonoBehaviour
{
    [SerializeField]
    GameObject playerItemPrefab;
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
                el.GetComponent<PlayerItem>().GetId() == key
                && el.GetComponent<PlayerItem>().GetName() != value
        );
    }

    public void removePlayerItems()
    {
        for (
            int i = playerItems.Count;
            i > SocketConnectionManager.Instance.selectedCharacters.Count;
            i--
        )
        {
            GameObject player = playerItems[i - 1];
            playerItems.RemoveAt(i - 1);
            Destroy(player);
        }
    }

    public void CreatePlayerItem(ulong id)
    {
        GameObject newPlayer = Instantiate(playerItemPrefab, gameObject.transform);
        PlayerItem playerI = newPlayer.GetComponent<PlayerItem>();
        playerI.SetId(id);
        playerI.SetCharacterName("No Selected");
        playerI.SetPlayerItemText();

        playerItems.Add(newPlayer);
    }

    public void UpdatePlayerItem(ulong id, string character)
    {
        if (playerItems.Count > 0)
        {
            PlayerItem playerI = playerItems
                ?.Find(el => el.GetComponent<PlayerItem>().GetId() == id)
                ?.GetComponent<PlayerItem>();
            playerI.SetCharacterName(character);
            playerI.SetPlayerItemText();
        }
    }
}
