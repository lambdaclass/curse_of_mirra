using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionList : MonoBehaviour
{
    [SerializeField]
    GameObject playerItemPrefab;
    public List<GameObject> playerItems = new List<GameObject>();


    public void DisplayPlayerItems()
    {
        if (playerItems.Count < SocketConnectionManager.Instance.selectedCharacters?.Count)
        {
            foreach (KeyValuePair<ulong, string> entry in SocketConnectionManager.Instance.selectedCharacters)
            {
                if (entry.Key != (ulong)LobbyConnection.Instance.playerId)
                {
                    CreatePlayerItem(entry.Key);
                }
            }
        }
    }

    public void DisplayUpdates()
    {
        if (SocketConnectionManager.Instance.selectedCharacters?.Count > 0)
        {
            foreach (KeyValuePair<ulong, string> entry in SocketConnectionManager.Instance.selectedCharacters)
            {
                UpdatePlayerItem(entry.Key, entry.Value);
            }
        }
    }

    public void removePlayerItems()
    {
        for (int i = playerItems.Count; i > SocketConnectionManager.Instance.selectedCharacters.Count; i--)
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

        if (id == 1)
        {
            playerI.playerText.text += $"{id.ToString()} {character} HOST";
        }
        else
        {
            if (SocketConnectionManager.Instance.playerId == id)
            {
                playerI.playerText.text += $"{id.ToString()} {character} YOU";
            }
            else
            {
                playerI.playerText.text += $"{id.ToString()} {character}";
            }
        }
        playerItems.Add(newPlayer);
    }

    public void UpdatePlayerItem(ulong id, string character)
    {
        if (playerItems.Count > 0)
        {
            PlayerItem playerI = playerItems?.Find(el => el.GetComponent<PlayerItem>().GetId() == id).GetComponent<PlayerItem>();

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
            foreach (KeyValuePair<ulong, string> entry in SocketConnectionManager.Instance.selectedCharacters)
            {
                if (entry.Key == id) character = entry.Value;
            }
        }
        return character;
    }

}
