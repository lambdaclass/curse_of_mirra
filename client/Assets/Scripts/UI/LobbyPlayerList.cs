using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LobbyPlayerList : MonoBehaviour
{
    [SerializeField]
    GameObject playerItemPrefab;

    [SerializeField]
    GameObject playButton;
    // List<GameObject> playerItems = new List<GameObject>();
    Dictionary<ulong, GameObject> playerItems = new Dictionary<ulong, GameObject>();

    // Update is called once per frame
    void Update()
    {
        var removed_keys = this.playerItems.Keys.Except(LobbyConnection.Instance.playersIdName.Keys).ToList();
        foreach (ulong key in removed_keys)
        {
            removePlayerItem(key);
        }

        if (removed_keys.Count > 0)
        {
            updatePlayerItem(LobbyConnection.Instance.hostId);
        }

        foreach (KeyValuePair<ulong, string> kvp in LobbyConnection.Instance.playersIdName)
        {
            if (!this.playerItems.ContainsKey(kvp.Key))
            {
                createPlayerItem(kvp.Key, kvp.Value);
            }
        }
    }

    public void createPlayerItem(ulong id, string name)
    {
        GameObject newPlayer = Instantiate(playerItemPrefab, gameObject.transform);
        PlayerItem playerI = newPlayer.GetComponent<PlayerItem>();
        playerI.SetId(id);
        playerI.SetPlayerItemText(name);

        this.playerItems[id] = newPlayer;
    }

    public void removePlayerItem(ulong id)
    {
        GameObject player = this.playerItems[id];
        this.playerItems.Remove(id);
        Destroy(player);
    }

    public void updatePlayerItem(ulong id)
    {
        GameObject player = this.playerItems[id];
        PlayerItem playerI = player.GetComponent<PlayerItem>();
        playerI.updateText();
    }
}
