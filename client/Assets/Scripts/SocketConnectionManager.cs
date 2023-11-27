using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Google.Protobuf;
using Google.Protobuf.Collections;
using NativeWebSocket;
using UnityEngine;

public class SocketConnectionManager : MonoBehaviour
{
    public List<GameObject> players;

    public Dictionary<int, GameObject> projectiles = new Dictionary<int, GameObject>();
    public static Dictionary<int, GameObject> projectilesStatic;

    [Tooltip("Session ID to connect to. If empty, a new session will be created")]
    public string sessionId = "";

    [Tooltip("IP to connect to. If empty, localhost will be used")]
    public string serverIp = "localhost";
    public static SocketConnectionManager Instance;
    public List<Player> gamePlayers;
    public GameEvent gameEvent;
    public List<Projectile> gameProjectiles;
    public Dictionary<ulong, string> selectedCharacters;
    public ulong playerId;
    public uint currentPing;
    public uint serverTickRate_ms;
    public string serverHash;
    public (Player, ulong) winnerPlayer = (null, 0);

    public List<Player> winners = new List<Player>();
    public Dictionary<ulong, string> playersIdName = new Dictionary<ulong, string>();

    public ClientPrediction clientPrediction = new ClientPrediction();

    public List<GameEvent> gameEvents = new List<GameEvent>();

    public EventsBuffer eventsBuffer;
    public bool allSelected = false;

    public float playableRadius;
    public Position shrinkingCenter;

    public List<Player> alivePlayers = new List<Player>();
    public List<LootPackage> updatedLoots = new List<LootPackage>();

    public bool cinematicDone;

    public bool connected = false;

    WebSocket ws;

    private string clientId;
    private bool reconnect;

    public class Session
    {
        public string sessionId { get; set; }
    }

    // public void Awake()
    // {
    //     Init();
    // }

    public void Init()
    {
        StartCoroutine(WaitForLobbyConnection());
        if (Instance != null)
        {
            if (this.ws != null)
            {
                this.ws.Close();
            }
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            this.sessionId = LobbyConnection.Instance.GameSession;
            this.serverIp = LobbyConnection.Instance.serverIp;
            this.serverTickRate_ms = LobbyConnection.Instance.serverTickRate_ms;
            this.serverHash = LobbyConnection.Instance.serverHash;
            this.clientId = LobbyConnection.Instance.clientId;
            this.reconnect = LobbyConnection.Instance.reconnect;
            this.playersIdName = LobbyConnection.Instance.playersIdName;

            projectilesStatic = this.projectiles;
            DontDestroyOnLoad(gameObject);

            if (this.reconnect)
            {
                this.selectedCharacters = LobbyConnection.Instance.reconnectPlayers;
                this.allSelected = !LobbyConnection.Instance.reconnectToCharacterSelection;
                this.cinematicDone = true;
            }
        }
    }

    private IEnumerator WaitForLobbyConnection()
    {
        yield return new WaitUntil(() => LobbyConnection.Instance != null);
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        if (ws != null)
        {
            ws.DispatchMessageQueue();
        }
#endif

        StartCoroutine(IsGameCreated());
    }

    private IEnumerator IsGameCreated()
    {
        yield return new WaitUntil(
            () =>
                LobbyConnection.Instance.GameSession != ""
                && LobbyConnection.Instance.GameSession != null
        );
        this.sessionId = LobbyConnection.Instance.GameSession;
        this.serverIp = LobbyConnection.Instance.serverIp;
        this.serverTickRate_ms = LobbyConnection.Instance.serverTickRate_ms;
        this.serverHash = LobbyConnection.Instance.serverHash;
        this.clientId = LobbyConnection.Instance.clientId;
        this.reconnect = LobbyConnection.Instance.reconnect;

        if (!connected && this.sessionId != "")
        {
            ConnectToSession(this.sessionId);
            connected = true;
            eventsBuffer = new EventsBuffer { deltaInterpolationTime = 100 };
        }
    }

    private void ConnectToSession(string sessionId)
    {
        string url = makeWebsocketUrl(
            "/play/" + sessionId + "/" + this.clientId + "/" + "delete-this"
        );
        print(url);
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("dark-worlds-client-hash", GitInfo.GetGitHash());
        ws = new WebSocket(url, headers);
        ws.OnMessage += OnWebSocketMessage;
        ws.OnClose += onWebsocketClose;
        ws.OnError += (e) =>
        {
            Debug.Log("Received error: " + e);
        };
        ws.Connect();
    }

    private void OnWebSocketMessage(byte[] data)
    {
        try
        {
            GameEvent gameEvent = GameEvent.Parser.ParseFrom(data);
            switch (gameEvent.Type)
            {
                case GameEventType.StateUpdate:
                    this.playableRadius = gameEvent.PlayableRadius;
                    this.shrinkingCenter = gameEvent.ShrinkingCenter;
                    KillFeedManager.instance.putEvents(gameEvent.Killfeed.ToList());
                    this.gamePlayers = gameEvent.Players.ToList();
                    eventsBuffer.AddEvent(gameEvent);
                    this.gameProjectiles = gameEvent.Projectiles.ToList();
                    alivePlayers = gameEvent.Players.ToList().FindAll(el => el.Health > 0);
                    updatedLoots = gameEvent.Loots.ToList();
                    break;
                case GameEventType.PingUpdate:
                    currentPing = (uint)gameEvent.Latency;
                    break;
                case GameEventType.GameFinished:
                    winnerPlayer.Item1 = gameEvent.WinnerPlayer;
                    winnerPlayer.Item2 = gameEvent.WinnerPlayer.KillCount;
                    this.gamePlayers = gameEvent.Players.ToList();
                    break;
                case GameEventType.PlayerJoined:
                    this.playerId = gameEvent.PlayerJoinedId;
                    break;
                default:
                    print("Message received is: " + gameEvent.Type);
                    break;
            }
        }
        catch (Exception e)
        {
            Debug.Log("InvalidProtocolBufferException: " + e);
        }
    }

    private void onWebsocketClose(WebSocketCloseCode closeCode)
    {
        Debug.Log("closeCode:" + closeCode);
        if (closeCode != WebSocketCloseCode.Normal)
        {
            LobbyConnection.Instance.errorConnection = true;
            this.Init();
            LobbyConnection.Instance.Init();
        }
    }

    public Dictionary<ulong, string> fromMapFieldToDictionary(MapField<ulong, string> dict)
    {
        Dictionary<ulong, string> result = new Dictionary<ulong, string>();

        foreach (KeyValuePair<ulong, string> element in dict)
        {
            result.Add(element.Key, element.Value);
        }

        return result;
    }

    public static Player GetPlayer(ulong id, List<Player> playerList)
    {
        return playerList.Find(el => el.Id == id);
    }

    public void SendAction(ClientAction action)
    {
        using (var stream = new MemoryStream())
        {
            action.WriteTo(stream);
            var msg = stream.ToArray();
            ws.Send(msg);
        }
    }

    public void SendGameAction<T>(IMessage<T> action)
        where T : IMessage<T>
    {
        using (var stream = new MemoryStream())
        {
            action.WriteTo(stream);
            var msg = stream.ToArray();
            ws.Send(msg);
        }
    }

    private string makeUrl(string path)
    {
        var useProxy = LobbyConnection.Instance.serverSettings.RunnerConfig.UseProxy;
        int port;

        if (useProxy == "true")
        {
            port = 5000;
        }
        else
        {
            port = 4000;
        }

        if (serverIp.Contains("localhost"))
        {
            return "http://" + serverIp + ":" + port + path;
        }
        else if (serverIp.Contains("10.150.20.186"))
        {
            return "http://" + serverIp + ":" + port + path;
        }
        else
        {
            return "https://" + serverIp + path;
        }
    }

    private string makeWebsocketUrl(string path)
    {
        // var useProxy = LobbyConnection.Instance.serverSettings.RunnerConfig.UseProxy;

        int port = 4000;

        // if (useProxy == "true")
        // {
        //     port = 5000;
        // }
        // else
        // {
        //     port = 4000;
        // }

        if (serverIp.Contains("localhost"))
        {
            return "ws://" + serverIp + ":" + port + path;
        }
        else if (serverIp.Contains("10.150.20.186"))
        {
            return "ws://" + serverIp + ":" + port + path;
        }
        else
        {
            return "wss://" + serverIp + path;
        }
    }

    public void closeConnection()
    {
        ws.Close();
    }

    public bool isConnectionOpen()
    {
        return ws.State == NativeWebSocket.WebSocketState.Open;
    }

    public bool GameHasEnded()
    {
        return winnerPlayer.Item1 != null;
    }

    public bool PlayerIsWinner(ulong playerId)
    {
        return GameHasEnded() && winnerPlayer.Item1.Id == playerId;
    }
}
