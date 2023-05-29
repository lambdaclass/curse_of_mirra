using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPlayerList : MonoBehaviour
{
    [SerializeField]
    GameObject playerItemPrefab;

    [SerializeField]
    GameObject playButton;
    int totalPlayersBefore = 0;

    List<PlayerItem> playerItems = new List<PlayerItem>();

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < LobbyConnection.Instance.playerCount; i++)
        {
            if (totalPlayersBefore != LobbyConnection.Instance.playerCount)
            {
                totalPlayersBefore++;
                CreatePlayerItem(totalPlayersBefore);
            }
        }
    }

    private void CreatePlayerItem(int id)
    {
        GameObject newPlayer = Instantiate(playerItemPrefab, gameObject.transform);
        PlayerItem playerI = newPlayer.GetComponent<PlayerItem>();

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
        playerI.id = id;
        playerItems.Add(playerI);
    }

    public PlayerItem GetPlayerCharacter(int id){
        PlayerItem item = new PlayerItem();
        for(int i = 0;i<playerItems.Count;i++){
            if(id == playerItems[i].GetId()){
                item = playerItems[i];
            }
        }
        return item;
    }
}
