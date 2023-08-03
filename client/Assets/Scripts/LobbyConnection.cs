using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Google.Protobuf;
using NativeWebSocket;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyConnection : MonoBehaviour
{
    [Tooltip("IP to connect to. If empty, localhost will be used")]
    public string server_name = "LocalHost";
    public string server_ip = "localhost";
    public List<string> lobbiesList;
    public List<string> gamesList;
    public static LobbyConnection Instance;
    public string GameSession;
    public string LobbySession;
    public ulong playerId;
    public int playerCount;
    public uint serverTickRate_ms;
    public string serverHash;
    public ServerGameSettings serverSettings;
    public List<GameObject> totalLobbyPlayers = new List<GameObject>();

    public bool gameStarted = false;

    WebSocket ws;

    [Serializable]
    public class Session
    {
        public string lobby_id;
    }

    [Serializable]
    public class LobbiesResponse
    {
        public List<string> lobbies;
    }

    [Serializable]
    public class GamesResponse
    {
        public List<string> current_games;
    }

    class AcceptAllCertificates : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }

    private void Awake()
    {
        this.Init();
        PopulateLists();
    }

    public void Init()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        this.server_ip = SelectServerIP.GetServerIp();
        this.server_name = SelectServerIP.GetServerName();
        this.playerId = UInt64.MaxValue;
        DontDestroyOnLoad(gameObject);
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

    private void PopulateLists()
    {
        this.lobbiesList = new List<string>();
        this.gamesList = new List<string>();
        StartCoroutine(GetLobbies());
        StartCoroutine(GetGames());
    }

    public void CreateLobby()
    {
        StartCoroutine(GetRequest(makeUrl("/new_lobby")));
    }

    public void ConnectToLobby(string matchmaking_id)
    {
        ConnectToSession(matchmaking_id);
        LobbySession = matchmaking_id;
    }

    public void Refresh()
    {
        this.server_ip = SelectServerIP.GetServerIp();
        this.server_name = SelectServerIP.GetServerName();
        PopulateLists();
    }

    public void QuickGame()
    {
        StartCoroutine(GetRequest(makeUrl("/new_lobby")));
        StartCoroutine(WaitLobbyCreated());
    }

    public IEnumerator StartGame()
    {
        yield return GameSettings.ParseSettingsCoroutine(settings =>
        {
            serverSettings = settings;
        });
        LobbyEvent lobbyEvent = new LobbyEvent
        {
            Type = LobbyEventType.StartGame,
            GameConfig = serverSettings
        };

        serverTickRate_ms = (uint)serverSettings.RunnerConfig.ServerTickrateMs;

        using (var stream = new MemoryStream())
        {
            lobbyEvent.WriteTo(stream);
            var msg = stream.ToArray();
            ws.Send(msg);
        }
    }

    private IEnumerator WaitLobbyCreated()
    {
        yield return new WaitUntil(() => !string.IsNullOrEmpty(LobbySession));
        yield return StartGame();
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            webRequest.certificateHandler = new AcceptAllCertificates();
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
                    Debug.Log("Creating and joining lobby ID: " + session.lobby_id);
                    ConnectToSession(session.lobby_id);
                    break;
            }
        }
    }

    IEnumerator GetLobbies()
    {
        string url = makeUrl("/current_lobbies");
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.certificateHandler = new AcceptAllCertificates();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                case UnityWebRequest.Result.ProtocolError:
                    break;
                case UnityWebRequest.Result.Success:
                    var response = JsonUtility.FromJson<LobbiesResponse>(
                        webRequest.downloadHandler.text
                    );
                    lobbiesList = response.lobbies;
                    break;
            }
        }
    }

    IEnumerator GetGames()
    {
        string url = makeUrl("/current_games");
        Debug.Log(url);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.certificateHandler = new AcceptAllCertificates();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    GamesResponse response = JsonUtility.FromJson<GamesResponse>(
                        webRequest.downloadHandler.text
                    );
                    gamesList = response.current_games;
                    break;
                default:
                    break;
            }
        }
    }

    private void ConnectToSession(string session_id)
    {
        string url = makeWebsocketUrl("/matchmaking/" + session_id);
        ws = new WebSocket(url);
        ws.OnMessage += OnWebSocketMessage;
        ws.OnError += (e) =>
        {
            Debug.Log("Error received: " + e);
        };
        ws.OnOpen += () =>
        {
            LobbySession = session_id;
        };
        ws.Connect();
    }

    private void OnWebSocketMessage(byte[] data)
    {
        try
        {
            LobbyEvent lobby_event = LobbyEvent.Parser.ParseFrom(data);
            switch (lobby_event.Type)
            {
                case LobbyEventType.Connected:
                    Debug.Log(
                        "Connected to lobby "
                            + lobby_event.LobbyId
                            + " as player_id "
                            + lobby_event.PlayerId
                    );
                    break;

                case LobbyEventType.PlayerAdded:
                    if (playerId == UInt64.MaxValue)
                    {
                        playerId = lobby_event.AddedPlayerId;
                    }
                    playerCount = lobby_event.Players.Count();
                    break;

                case LobbyEventType.PlayerRemoved:
                    playerCount = lobby_event.Players.Count();
                    break;

                case LobbyEventType.GameStarted:
                    GameSession = lobby_event.GameId;
                    serverSettings = lobby_event.GameConfig;
                    serverTickRate_ms = (uint)serverSettings.RunnerConfig.ServerTickrateMs;
                    serverHash = lobby_event.ServerHash;
                    gameStarted = true;
                    break;

                default:
                    Debug.Log("Message received is: " + lobby_event.Type);
                    break;
            }
            ;
        }
        catch (Exception e)
        {
            Debug.Log("InvalidProtocolBufferException: " + e);
        }
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

    public bool isConnectionOpen()
    {
        return ws.State == NativeWebSocket.WebSocketState.Open;
    }
}
