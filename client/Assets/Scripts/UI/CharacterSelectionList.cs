using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionList : MonoBehaviour
{
  [SerializeField]
  GameObject playerItemPrefab;
  List<GameObject> playerItems = new List<GameObject>();

  // Update is called once per frame
  void Update()
  {
    if (playerItems.Count < SocketConnectionManager.Instance.selectedCharacters.Count)
    {
      createPlayerItems();
    }
    else if (playerItems.Count > SocketConnectionManager.Instance.selectedCharacters.Count)
    {
      removePlayerItems();
    }
  }

  private void createPlayerItems()
  {
    for (int i = playerItems.Count; i < SocketConnectionManager.Instance.selectedCharacters.Count; i++)
    {
      playerItems.Add(CreatePlayerItem(i + 1));
    }
  }

  private void removePlayerItems()
  {
    for (int i = playerItems.Count; i > SocketConnectionManager.Instance.selectedCharacters.Count; i--)
    {
      GameObject player = playerItems[i - 1];
      playerItems.RemoveAt(i - 1);
      Destroy(player);
    }
  }

  private GameObject CreatePlayerItem(int id)
  {
    GameObject newPlayer = Instantiate(playerItemPrefab, gameObject.transform);
    PlayerItem playerI = newPlayer.GetComponent<PlayerItem>();
    PlayerCharacter playerCharacter = GetPlayerCharacter(id);
    
    if (id == 1)
    {
      playerI.playerText.text += $"{id.ToString()} {playerCharacter.CharacterName} HOST";
    }
    else
    {
      if (SocketConnectionManager.Instance.playerId == id)
      {
      playerI.playerText.text += $"{id.ToString()} {playerCharacter.CharacterName} YOU";
      }
      else
      {
      playerI.playerText.text += $"{id.ToString()} {playerCharacter.CharacterName}";
      }
    }

    return newPlayer;
  }

  PlayerCharacter GetPlayerCharacter(int id)
  {
    return SocketConnectionManager.Instance.selectedCharacters.Find(
        el => (int) el.PlayerId == id
    );
  }

}
