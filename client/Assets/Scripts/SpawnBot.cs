using MoreMountains.Tools;
using UnityEngine;
using System.Collections.Generic;

public class SpawnBot : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> playerPrefab;

    private bool pendingSpawn = false;
    private bool botsActive = true;
    private Vector3 spawnPosition = new Vector3(0, 0, 0);
    private string botId;

    public static SpawnBot Instance;

    public void Awake()
    {
        Init();
    }

    public void Init()
    {
        Instance = this;
        if (SocketConnectionManager.Instance.players.Count == 9)
        {
            GetComponent<MMTouchButton>().DisableButton();
        }
    }

    public void GenerateBotPlayer()
    {
        SocketConnectionManager.Instance.CallSpawnBot();
        if (SocketConnectionManager.Instance.players.Count == 9)
        {
            GetComponent<MMTouchButton>().DisableButton();
        }
    }

    public void ToggleBots()
    {
        botsActive = !botsActive;
        SocketConnectionManager.Instance.ToggleBots();
        GetComponent<ToggleButton>().ToggleWithSiblingComponentBool(botsActive);
    }

    public CustomCharacter GetCharacterByName(string name){
       return playerPrefab.Find(el => el.name == name).GetComponent<CustomCharacter>();
    }
    
}
