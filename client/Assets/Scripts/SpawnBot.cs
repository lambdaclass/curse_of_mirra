using MoreMountains.Tools;
using UnityEngine;

public class SpawnBot : MonoBehaviour
{
    [SerializeField]
    public GameObject playerPrefab;

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
}
