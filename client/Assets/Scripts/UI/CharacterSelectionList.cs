using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterSelectionList : MonoBehaviour
{
    [SerializeField]
    GameObject playerItemPrefab;
    public List<GameObject> playerItems = new List<GameObject>();

    // This create a new item for new selected character
    public void DisplayPlayerItems()
    {
        if (playerItems.Count < SocketConnectionManager.Instance.selectedCharacters?.Count)
        {
            // foreach (
            //     KeyValuePair<ulong, string> entry in SocketConnectionManager
            //         .Instance
            //         .selectedCharacters
            // )
            // {
            //     if (entry.Key != (ulong)LobbyConnection.Instance.playerId)
            //     {
            //         playerItems.ForEach(el =>
            //         {
            //             if (el.GetComponent<PlayerItem>().GetId() != entry.Key)
            //             {
            //                 created = true;
            //             }
            //         });
            //         if (created)
            //         {
            //             print("Created item for the player + " + entry.Key);
            //             CreatePlayerItem(entry.Key);
            //         }
            //     }
            // }
            print(SocketConnectionManager.Instance.selectedCharacters.Keys.Last());
            ulong lastKey = SocketConnectionManager.Instance.selectedCharacters
                .OrderBy(x => x.Key)
                .Last()
                .Key;
            print(lastKey);
            CreatePlayerItem(SocketConnectionManager.Instance.selectedCharacters.Keys.Last());
        }
    }

    //This updated the item for respective player
    public void DisplayUpdates()
    {
        // if (SocketConnectionManager.Instance.selectedCharacters?.Count > 0 && playerItems.Count > 0)
        // {
        //     foreach (
        //         KeyValuePair<ulong, string> entry in SocketConnectionManager
        //             .Instance
        //             .selectedCharacters
        //     )
        //     {
        //         if (
        //             entry.Key != (ulong)LobbyConnection.Instance.playerId
        //             && GetUpdatedItem(entry.Key, entry.Value)
        //         )
        //         {
        //             print("Updated item for the player + " + entry.Key);
        //             UpdatePlayerItem(entry.Key, entry.Value);
        //         }
        //     }
        // }
        if (
            SocketConnectionManager.Instance.selectedCharacters?.Count > 0 && playerItems?.Count > 0
        )
        {
            foreach (
                KeyValuePair<ulong, string> entry in SocketConnectionManager
                    .Instance
                    .selectedCharacters
            )
            {
                if (
                    entry.Key != (ulong)LobbyConnection.Instance.playerId
                    && GetUpdatedItem(entry.Key, entry.Value)
                )
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
        string character = GetPlayerCharacter(id);
        playerI.SetName(character);

        if (id == 1)
        {
            playerI.playerText.text += $" {id.ToString()} {character} HOST";
        }
        else
        {
            if (SocketConnectionManager.Instance.playerId == id)
            {
                playerI.playerText.text += $" {id.ToString()} {character} YOU";
            }
            else
            {
                playerI.playerText.text += $" {id.ToString()} {character}";
            }
        }
        playerItems.Add(newPlayer);
    }

    public void UpdatePlayerItem(ulong id, string character)
    {
        if (playerItems.Count > 0)
        {
            PlayerItem playerI = playerItems
                ?.Find(el => el.GetComponent<PlayerItem>().GetId() == id)
                ?.GetComponent<PlayerItem>();

            if (id == 1)
            {
                playerI.playerText.text = $"Player {id.ToString()} {character} HOST";
            }
            else
            {
                if (SocketConnectionManager.Instance.playerId == id)
                {
                    playerI.playerText.text = $"Player {id.ToString()} {character} YOU";
                }
                else
                {
                    playerI.playerText.text = $"Player {id.ToString()} {character}";
                }
            }
        }
    }

    public string GetPlayerCharacter(ulong id)
    {
        string character = null;
        if (SocketConnectionManager.Instance.selectedCharacters != null)
        {
            foreach (
                KeyValuePair<ulong, string> entry in SocketConnectionManager
                    .Instance
                    .selectedCharacters
            )
            {
                if (entry.Key == id)
                    character = entry.Value;
            }
        }
        return character;
    }
}
