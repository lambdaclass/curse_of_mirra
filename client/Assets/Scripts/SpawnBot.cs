using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;
using UnityEngine.UI;

public class SpawnBot : MonoBehaviour
{
    [SerializeField]
    GameObject playerPrefab;

    private bool pendingSpawn = false;
    private bool botsActive = true;
    private Vector3 spawnPosition = new Vector3(0, 0, 0);
    private string botId;

    public static SpawnBot Instance;
    private static readonly int _healthBarIndex = 1;

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
            playerPrefab.GetComponent<CustomCharacter>().PlayerID = "";

            CustomCharacter newPlayer = Instantiate(
                playerPrefab.GetComponent<CustomCharacter>(),
                spawnPosition,
                Quaternion.identity
            );
            newPlayer.PlayerID = botId.ToString();
            newPlayer.name = "BOT" + botId;
            Image healthBarFront = newPlayer.GetComponentsInChildren<MMProgressBar>()[
                _healthBarIndex
            ].ForegroundBar.GetComponent<Image>();

            healthBarFront.color = Utils.healthBarRed;

            SocketConnectionManager.Instance.players.Add(newPlayer.gameObject);

            pendingSpawn = false;
        }
    }
}
