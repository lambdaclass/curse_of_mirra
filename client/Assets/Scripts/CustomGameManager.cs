using System;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class CustomGameManager : MonoBehaviour
{
    private int totalPlayers;
    private int playerCount = 0;
    private int playerId;
    public LevelManager levelManager;
    public Character prefab;
    public CinemachineCameraController camera;

    void Awake()
    {
        this.totalPlayers = LobbyConnection.Instance.playerCount;
    }

    void Start()
    {
        GeneratePlayer();
        playerId = LobbyConnection.Instance.playerId;
        setCameraToPlayer(playerId);
    }

    public void GeneratePlayer()
    {
        for (int i = 0; i < totalPlayers; i++)
        {
            if (LobbyConnection.Instance.playerId == i + 1)
            {
                // Player1 is the ID to match with the client InputManager
                prefab.PlayerID = "Player1";
            }
            else
            {
                prefab.PlayerID = "";
            }
            Character newPlayer = Instantiate(
                prefab,
                levelManager.InitialSpawnPoint.transform.position,
                Quaternion.identity
            );
            newPlayer.name = "Player" + " " + (i + 1);
            newPlayer.PlayerID = (i + 1).ToString();

            SocketConnectionManager.playersStatic.Add(newPlayer.gameObject);
            levelManager.Players.Add(newPlayer);
        }
        levelManager.PlayerPrefabs = (levelManager.Players).ToArray();
    }

    private void setCameraToPlayer(int playerID)
    {
        foreach (Character player in levelManager.PlayerPrefabs)
        {
            if (Int32.Parse(player.PlayerID) == playerID)
            {
                this.camera.SetTarget(player);
                this.camera.StartFollowing();
            }
        }
    }
}
