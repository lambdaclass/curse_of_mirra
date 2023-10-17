using MoreMountains.Tools;
using UnityEngine;

public class SpawnBot : MonoBehaviour
{
    [SerializeField]
    GameObject playerPrefab;

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
        //GenerateBotPlayer();
    }

    public void GenerateBotPlayer()
    {
        SocketConnectionManager.Instance.CallSpawnBot();
    }

    public void ToggleBots()
    {
        botsActive = !botsActive;
        SocketConnectionManager.Instance.ToggleBots();
        GetComponent<ToggleButton>().ToggleWithSiblingComponentBool(botsActive);
    }

    public void Spawn(Player player)
    {
        pendingSpawn = true;
        spawnPosition = Utils.transformBackendPositionToFrontendPosition(player.Position);
        botId = player.Id.ToString();
    }

    public void Update()
    {
        if (pendingSpawn)
        {
            Debug.Log("Paso por acá");
            playerPrefab.GetComponent<CustomCharacter>().PlayerID = "";

            CustomCharacter newPlayer = Instantiate(
                playerPrefab.GetComponent<CustomCharacter>(),
                spawnPosition,
                Quaternion.identity
            );
            newPlayer.PlayerID = botId.ToString();
            newPlayer.name = "BOT" + botId;
            SocketConnectionManager.Instance.players.Add(newPlayer.gameObject);
            if (SocketConnectionManager.Instance.players.Count == 9)
            {
                GetComponent<MMTouchButton>().DisableButton();
            }

            pendingSpawn = false;
        }
    }
}
