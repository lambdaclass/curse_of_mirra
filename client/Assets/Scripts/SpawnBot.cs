using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class SpawnBot : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] SocketConnectionManager manager;

    public void Init()
    {
        GenerateBotPlayer();
    }

    public void GenerateBotPlayer()
    {
        playerPrefab.GetComponent<Character>().PlayerID = "";

        Character newPlayer = Instantiate(
            playerPrefab.GetComponent<Character>(),
            new Vector3(0, 0, 0),
            Quaternion.identity
        );
        newPlayer.PlayerID = "BOT" + " " + (manager.players.Count - 1);
        newPlayer.name = "BOT" + (manager.players.Count).ToString();
        manager.players.Add(newPlayer.gameObject);
    }
}
