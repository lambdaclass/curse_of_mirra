using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Google.Protobuf;
using Google.Protobuf.Collections;
using NativeWebSocket;
using UnityEngine;

public class GameServerConnectionManager : MonoBehaviour
{
    public List<GameObject> players;
    public Dictionary<int, GameObject> projectiles = new Dictionary<int, GameObject>();

    [Tooltip("Session ID to connect to. If empty, a new session will be created")]
    public string sessionId = "";

    [Tooltip("IP to connect to. If empty, localhost will be used")]
    public string serverIp = "localhost";

    public static GameServerConnectionManager Instance;

    public Dictionary<ulong, Position> playersIdPosition = new Dictionary<ulong, Position>();

    public List<Entity> gamePlayers;

    //     public OldGameEvent gameEvent;
    //     public List<OldProjectile> gameProjectiles;
    public ulong playerId;
    public uint currentPing;
    public float serverTickRate_ms;
    public string serverHash;

    public (Entity, ulong) winnerPlayer = (null, 0);
    public Dictionary<ulong, string> playersIdName = new Dictionary<ulong, string>();
    public ClientPrediction clientPrediction = new ClientPrediction();
    public EventsBuffer eventsBuffer = new EventsBuffer { deltaInterpolationTime = 100 };
    public bool allSelected = false;
    public float playableRadius;

    //     public OldPosition shrinkingCenter;
    //     public List<OldPlayer> alivePlayers = new List<OldPlayer>();
    public bool cinematicDone;
    public bool connected = false;

    //     public Game.GameState gameState;
    private string clientId;
    private bool reconnect;
    WebSocket ws;

    void Start()
    {
        Init();
    }

    public void Init()
    {
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
            this.sessionId = ServerConnection.Instance.GameSession;
            this.serverIp = ServerConnection.Instance.serverIp;
            this.clientId = ServerConnection.Instance.clientId;
            this.reconnect = ServerConnection.Instance.reconnect;
            this.playersIdName = ServerConnection.Instance.playersIdName;
            DontDestroyOnLoad(gameObject);
        }
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
        yield return new WaitUntil(() => !String.IsNullOrEmpty(SessionParameters.GameId));

        // this.sessionId = ServerConnection.Instance.GameSession;
        // this.serverIp = ServerConnection.Instance.serverIp;
        // this.serverTickRate_ms = ServerConnection.Instance.serverTickRate_ms;
        // this.serverHash = ServerConnection.Instance.serverHash;
        // this.clientId = ServerConnection.Instance.clientId;
        // this.reconnect = ServerConnection.Instance.reconnect;

        if (!connected)
        {
            connected = true;
            ConnectToSession(this.sessionId);
        }
    }

    private void ConnectToSession(string sessionId)
    {
        string url = makeWebsocketUrl(
            "/play/" + SessionParameters.GameId + "/" + SessionParameters.PlayerId
        );
        print(url);
        ws = new WebSocket(url);
        ws.OnMessage += OnWebSocketMessage;
        ws.OnClose += OnWebsocketClose;
        ws.OnError += (e) =>
        {
            Debug.Log("Received error: " + e);
        };
        ws.OnOpen += () =>
        {
            // Once the connection is established we reset so when we try to load the scenes again
            // it waits to fetch it from the Lobby websocket and not reuse
            SessionParameters.GameId = null;
        };
        ws.Connect();
    }

    private void OnWebsocketClose(WebSocketCloseCode closeCode)
    {
        if (closeCode != WebSocketCloseCode.Normal)
        {
            // TODO: Add some error handle for when websocket closes unexpectedly
        } else
        {
            Debug.Log("Game websocket closed normally");
        }
    }

    private void OnWebSocketMessage(byte[] data)
    {
        try
        {
            GameEvent gameEvent = GameEvent.Parser.ParseFrom(data);

            switch (gameEvent.EventCase)
            {
                case GameEvent.EventOneofCase.Joined:
                    this.serverTickRate_ms = gameEvent.Joined.Config.Game.TickRateMs;
                    this.playerId = gameEvent.Joined.PlayerId;
                    break;
                case GameEvent.EventOneofCase.Ping:
                    currentPing = (uint)gameEvent.Ping.Latency;
                    break;
                case GameEvent.EventOneofCase.Update:
                    GameState gameState = gameEvent.Update;

                    eventsBuffer.AddEvent(gameState);

                    var position = gameState.Players[this.playerId].Position;
                    this.gamePlayers = gameState.Players.Values.ToList();
                    this.playersIdPosition = new Dictionary<ulong, Position>
                    {
                        [this.playerId] = position
                    };
                    break;
                case GameEvent.EventOneofCase.Finished:
                    Debug.Log("Finished!!");
                    winnerPlayer.Item1 = gameEvent.Finished.Winner;
                    // winnerPlayer.Item2 = gameEvent.Finished.Winner.Player.KillCount;
                    this.gamePlayers = gameEvent.Finished.Players.Values.ToList();
                    break;
                default:
                    print("Message received is: " + gameEvent.EventCase);
                    break;
            }

            //             TransitionGameEvent gameEvent = TransitionGameEvent.Parser.ParseFrom(data);

            //             // TODO: Fix missing NewGameEvent, current missing are
            //             //      - PING_UPDATE
            //             //      - PLAYER_JOINED
            //             if (
            //                 gameEvent.OldGameEvent.Type != GameEventType.PingUpdate
            //                 && gameEvent.OldGameEvent.Type != GameEventType.PlayerJoined
            //             )
            //             {
            //                 try
            //                 {
            //                     switch (gameEvent.NewGameEvent.EventCase)
            //                     {
            //                         case GameEvent.EventOneofCase.GameState:
            //                             gameState = new Game.GameState(gameEvent.NewGameEvent.GameState);
            //                             break;
            //                         default:
            //                             print("Unexpected message: " + gameEvent.NewGameEvent.EventCase);
            //                             break;
            //                     }
            //                 }
            //                 catch (Exception e)
            //                 {
            //                     Debug.Log(gameEvent);
            //                     Debug.Log(e);
            //                     throw e;
            //                 }
            //             }

            //             switch (gameEvent.OldGameEvent.Type)
            //             {
            //                 case GameEventType.StateUpdate:
            //                     this.playableRadius = gameEvent.OldGameEvent.PlayableRadius;
            //                     this.shrinkingCenter = gameEvent.OldGameEvent.ShrinkingCenter;
            //                     eventsBuffer.AddEvent(gameEvent.OldGameEvent);
            //                     this.gamePlayers = gameEvent.OldGameEvent.Players.ToList();
            //                     this.gameProjectiles = gameEvent.OldGameEvent.Projectiles.ToList();
            //                     alivePlayers = gameEvent
            //                         .OldGameEvent
            //                         .Players
            //                         .ToList()
            //                         .FindAll(el => el.Health > 0);
            //                     KillFeedManager.instance.putEvents(gameEvent.OldGameEvent.Killfeed.ToList());
            //                     break;
            //                 case GameEventType.PingUpdate:
            //                     currentPing = (uint)gameEvent.OldGameEvent.Latency;
            //                     break;
            //                 case GameEventType.GameFinished:
            //                     winnerPlayer.Item1 = gameEvent.OldGameEvent.WinnerPlayer;
            //                     winnerPlayer.Item2 = gameEvent.OldGameEvent.WinnerPlayer.KillCount;
            //                     this.gamePlayers = gameEvent.OldGameEvent.Players.ToList();
            //                     break;
            //                 case GameEventType.PlayerJoined:
            //                     this.playerId = gameEvent.OldGameEvent.PlayerJoinedId;
            //                     break;
            //                 case GameEventType.GameStarted:
            //                     this.playableRadius = gameEvent.OldGameEvent.PlayableRadius;
            //                     this.shrinkingCenter = gameEvent.OldGameEvent.ShrinkingCenter;
            //                     eventsBuffer.AddEvent(gameEvent.OldGameEvent);
            //                     this.gamePlayers = gameEvent.OldGameEvent.Players.ToList();
            //                     this.gameProjectiles = gameEvent.OldGameEvent.Projectiles.ToList();
            //                     ServerConnection.Instance.gameStarted = true;
            //                     break;
            //                 default:
            //                     print("Message received is: " + gameEvent.OldGameEvent.Type);
            //                     break;
            //             }
        }
        catch (Exception e)
        {
            Debug.Log("InvalidProtocolBufferException: " + e);
        }
    }

    //     public static OldPlayer GetPlayer(ulong id, List<OldPlayer> playerList)
    //     {
    //         return playerList.Find(el => el.Id == id);
    //     }

    //     public void SendAction(ClientAction action)
    //     {
    //         using (var stream = new MemoryStream())
    //         {
    //             action.WriteTo(stream);
    //             var msg = stream.ToArray();
    //             ws.Send(msg);
    //         }
    //     }

    public void SendMove(float x, float y, long timestamp)
    {
        Direction direction = new Direction { X = x, Y = y };
        Move moveAction = new Move { Direction = direction };
        GameAction gameAction = new GameAction { Move = moveAction, Timestamp = timestamp };
        SendGameAction(gameAction);
    }

    private void SendGameAction<T>(IMessage<T> action)
        where T : IMessage<T>
    {
        using (var stream = new MemoryStream())
        {
            action.WriteTo(stream);
            var msg = stream.ToArray();
            ws.Send(msg);
        }
    }

    private string makeWebsocketUrl(string path)
    {
        int port = 4000;

        if (serverIp.Contains("localhost"))
        {
            return "ws://" + serverIp + ":" + port + path;
        }
        else if (serverIp.Contains("10.150.20.186"))
        {
            return "ws://" + serverIp + ":" + port + path;
        }
        // Load test server
        else if (serverIp.Contains("168.119.71.104"))
        {
            return "ws://" + serverIp + ":" + port + path;
        }
        // Load test runner server
        else if (serverIp.Contains("176.9.26.172"))
        {
            return "ws://" + serverIp + ":" + port + path;
        }
        else
        {
            return "ws://" + serverIp + ":4000" + path;
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
