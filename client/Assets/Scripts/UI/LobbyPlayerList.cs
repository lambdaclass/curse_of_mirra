using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPlayerList : MonoBehaviour
{
    [SerializeField]
    GameObject playerItemPrefab;

    [SerializeField]
    GameObject playButton;
    List<GameObject> playerItems = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        if (playerItems.Count < LobbyConnection.Instance.playerCount)
        {
            createPlayerItems();
        }
        else if (playerItems.Count > LobbyConnection.Instance.playerCount)
        {
            removePlayerItems();
        }
    }

    private void createPlayerItems()
    {
        for (int i = playerItems.Count; i < LobbyConnection.Instance.playerCount; i++)
        {
            playerItems.Add(CreatePlayerItem((ulong)i + 1));
        }
    }

    private void removePlayerItems()
    {
        for (int i = playerItems.Count; i > LobbyConnection.Instance.playerCount; i--)
        {
            GameObject player = playerItems[i - 1];
            playerItems.RemoveAt(i - 1);
            Destroy(player);
        }
    }

    private GameObject CreatePlayerItem(ulong id)
    {
        GameObject newPlayer = Instantiate(playerItemPrefab, gameObject.transform);
        PlayerItem playerI = newPlayer.GetComponent<PlayerItem>();
        playerI.SetId(id);

        if (id == 1)
        {
            playerI.playerText.text += " " + (id.ToString() + " " + "HOST");
            playButton.SetActive(true);
        }
        else
        {
            if (LobbyConnection.Instance.playerId == id)
            {
                playerI.playerText.text += " " + id.ToString() + " " + "YOU";
            }
            else
            {
                playerI.playerText.text += " " + id.ToString();
            }
        }

        return newPlayer;
    }
}
