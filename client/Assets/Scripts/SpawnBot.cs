using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class SpawnBot : MonoBehaviour
{
    [SerializeField]
    GameObject playerPrefab;

    private bool pendingSpawn = false;
    private Vector3 spawnPosition = new Vector3(0, 0, 0);
    private string botId;

    public static SpawnBot Instance;

    public void Init()
    {
        if (SocketConnectionManager.Instance.players.Count == 9)
            GetComponent<MMTouchButton>().DisableButton();
        Instance = this;
        GenerateBotPlayer();
    }

    public void GenerateBotPlayer()
    {
        SocketConnectionManager.Instance.CallSpawnBot();
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
            playerPrefab.GetComponent<Character>().PlayerID = "";

            Character newPlayer = Instantiate(
                playerPrefab.GetComponent<Character>(),
                spawnPosition,
                Quaternion.identity
            );
            newPlayer.PlayerID = botId.ToString();
            newPlayer.name = "BOT" + botId;
            SocketConnectionManager.Instance.players.Add(newPlayer.gameObject);
            print("SPAWNED");
            print(newPlayer.PlayerID);

            pendingSpawn = false;
        }
    }
}
