using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class SpawnBot : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] SocketConnectionManager manager;

    private bool pendingSpawn = false;
    private bool botId;
    private int countBefore;

    public static SpawnBot Instance;

    public void Init()
    {
        if (manager.players.Count == 9) GetComponent<MMTouchButton>().DisableButton();
        Instance = this;
        GenerateBotPlayer();
        StartCoroutine(WaitForSpawn());
    }

    public void GenerateBotPlayer()
    {
        manager.CallSpawnBot();
        countBefore = manager.gamePlayers.Count;
    }

    IEnumerator WaitForSpawn()
    {
        yield return new WaitUntil(() => manager.gamePlayers.Count > countBefore);
        Spawn();
    }

    public void Spawn()
    {
        string botId = manager.players.Count.ToString();
        countBefore = manager.gamePlayers.Count;
        playerPrefab.GetComponent<Character>().PlayerID = "";

        Character newPlayer = Instantiate(
            playerPrefab.GetComponent<Character>(),
            new Vector3(0, 0, 0),
            Quaternion.identity
        );
        newPlayer.PlayerID = "BOT" + " " + botId;
        newPlayer.name = "BOT" + botId;
        manager.players.Add(newPlayer.gameObject);
        print("SPAWNED");
    }
}
