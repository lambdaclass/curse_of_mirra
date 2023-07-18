using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Google.Protobuf;
using Google.Protobuf.Collections;
using NativeWebSocket;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class SocketConnectionManager : MonoBehaviour
{
    public List<GameObject> players;

    public Dictionary<int, GameObject> projectiles = new Dictionary<int, GameObject>();
    public static Dictionary<int, GameObject> projectilesStatic;

    [Tooltip("Session ID to connect to. If empty, a new session will be created")]
    public string session_id = "";

    [Tooltip("IP to connect to. If empty, localhost will be used")]
    public string server_ip = "localhost";
    public static SocketConnectionManager Instance;
    public List<Player> gamePlayers;
    public GameEvent gameEvent;
    public List<Projectile> gameProjectiles;
    public Dictionary<ulong, string> selectedCharacters;
    public ulong playerId;
    public uint currentPing;
    public uint serverTickRate_ms;
    public Player winnerPlayer = null;

    public List<Player> winners = new List<Player>();

    public ClientPrediction clientPrediction = new ClientPrediction();

    public List<GameEvent> gameEvents = new List<GameEvent>();
    private Boolean botsActive = true;

    public EventsBuffer eventsBuffer;
    public bool allSelected = false;

    WebSocket ws;

    public class Session
    {
        public string session_id { get; set; }
    }

    public void Awake()
    {
        Init();
    }

    public void Init()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            this.session_id = LobbyConnection.Instance.GameSession;
            this.server_ip = LobbyConnection.Instance.server_ip;
            this.serverTickRate_ms = LobbyConnection.Instance.serverTickRate_ms;
            projectilesStatic = this.projectiles;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        playerId = LobbyConnection.Instance.playerId;
        ConnectToSession(this.session_id);
        eventsBuffer = new EventsBuffer { deltaInterpolationTime = 100 };
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        if (ws != null)
        {
            ws.DispatchMessageQueue();
        }
#endif
    }

    IEnumerator GetRequest()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(makeUrl("/new_session")))
        {
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                case UnityWebRequest.Result.ProtocolError:
                    break;
                case UnityWebRequest.Result.Success:
                    Session session = JsonUtility.FromJson<Session>(
                        webRequest.downloadHandler.text
                    );
                    print("Creating and joining Session ID: " + session.session_id);
                    ConnectToSession(session.session_id);
                    break;
            }
        }
    }

    private void ConnectToSession(string session_id)
    {
        string url = makeWebsocketUrl("/play/" + session_id + "/" + playerId);
        print(url);
        ws = new WebSocket(url);
        ws.OnMessage += OnWebSocketMessage;
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
            GameEvent game_event = GameEvent.Parser.ParseFrom(data);
            switch (game_event.Type)
            {
                case GameEventType.StateUpdate:
                    KillFeedManager.instance.putEvents(game_event.Killfeed.ToList());

                    if (
                        this.gamePlayers != null
                        && this.gamePlayers.Count < game_event.Players.Count
                    )
                    {
                        game_event.Players
                            .ToList()
                            .FindAll((player) => !this.gamePlayers.Contains(player))
                            .ForEach(
                                (player) =>
                                {
                                    SpawnBot.Instance.Spawn(player);
                                }
                            );
                    }
                    // This should be deleted when the match end is fixed
                    // game_event.Players.ToList().ForEach((player) => print("PLAYER: " + player.Id + " KILLS: " + player.KillCount + " DEATHS: " + player.DeathCount));
                    this.gamePlayers = game_event.Players.ToList();
                    eventsBuffer.AddEvent(game_event);
                    this.gameProjectiles = game_event.Projectiles.ToList();
                    break;
                case GameEventType.PingUpdate:
                    currentPing = (uint)game_event.Latency;
                    break;
                case GameEventType.NextRound:
                    print("The winner of the round is " + game_event.WinnerPlayer);
                    winners.Add(game_event.WinnerPlayer);
                    var newPlayer1 = GetPlayer(
                        SocketConnectionManager.Instance.playerId,
                        game_event.Players.ToList()
                    );

                    break;
                case GameEventType.LastRound:
                    winners.Add(game_event.WinnerPlayer);
                    print("The winner of the round is " + game_event.WinnerPlayer);
                    var newPlayer2 = GetPlayer(
                        SocketConnectionManager.Instance.playerId,
                        game_event.Players.ToList()
                    );

                    break;
                case GameEventType.GameFinished:
                    winnerPlayer = game_event.WinnerPlayer;
                    // This should be uncommented when the match end is finished
                    // game_event.Players.ToList().ForEach((player) => print("PLAYER: " + player.Id + " KILLS: " + player.KillCount + " DEATHS: " + player.DeathCount));
                    break;
                case GameEventType.InitialPositions:
                    this.gamePlayers = game_event.Players.ToList();
                    break;
                case GameEventType.SelectedCharacterUpdate:
                    this.selectedCharacters = fromMapFieldToDictionary(
                        game_event.SelectedCharacters
                    );
                    break;
                case GameEventType.FinishCharacterSelection:
                    this.selectedCharacters = fromMapFieldToDictionary(
                        game_event.SelectedCharacters
                    );
                    this.allSelected = true;
                    this.gamePlayers = game_event.Players.ToList();
                    break;
                default:
                    print("Message received is: " + game_event.Type);
                    break;
            }
        }
        catch (Exception e)
        {
            Debug.Log("InvalidProtocolBufferException: " + e);
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

    public static Player GetPlayer(ulong id, List<Player> player_list)
    {
        return player_list.Find(el => el.Id == id);
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

    public void CallSpawnBot()
    {
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        ClientAction clientAction = new ClientAction
        {
            Action = Action.AddBot,
            Timestamp = timestamp
        };
        SendAction(clientAction);
    }

    public void ToggleBots()
    {
        ClientAction clientAction;
        if (this.botsActive)
        {
            clientAction = new ClientAction { Action = Action.DisableBots };
        }
        else
        {
            clientAction = new ClientAction { Action = Action.EnableBots };
        }

        this.botsActive = !this.botsActive;
        SendAction(clientAction);
    }

    private string makeUrl(string path)
    {
        if (server_ip.Contains("localhost"))
        {
            return "http://" + server_ip + ":4000" + path;
        }
        else if (server_ip.Contains("10.150.20.186"))
        {
            return "http://" + server_ip + ":4000" + path;
        }
        else
        {
            return "https://" + server_ip + path;
        }
    }

    private string makeWebsocketUrl(string path)
    {
        if (server_ip.Contains("localhost"))
        {
            return "ws://" + server_ip + ":4000" + path;
        }
        else if (server_ip.Contains("10.150.20.186"))
        {
            return "ws://" + server_ip + ":4000" + path;
        }
        else
        {
            return "wss://" + server_ip + path;
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
}
