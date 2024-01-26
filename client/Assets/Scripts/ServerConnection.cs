using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Google.Protobuf;
using NativeWebSocket;
using UnityEngine;
using UnityEngine.Networking;

public class ServerConnection : MonoBehaviour
{
    public string serverName;
    public string serverIp;
    public static ServerConnection Instance;
    public string GameSession;
    public string LobbySession;
    public ulong playerId;
    public int playerCount;
    public int simulatedPlayerCount;
    public int lobbyCapacity;
    public Dictionary<ulong, string> playersIdName = new Dictionary<ulong, string>();
    public uint serverTickRate_ms;
    public string serverHash;

    // public ServerGameSettings serverSettings;
    // public Config engineServerSettings;
    public bool gameStarted = false;
    public bool errorOngoingGame = false;
    public bool errorConnection = false;
    public string clientId;
    public bool reconnect = false;
    public bool reconnectPossible = false;
    public bool reconnectToCharacterSelection = false;
    public int reconnectPlayerCount;
    public string reconnectServerHash;
    public string reconnectGameId;
    public ulong reconnectPlayerId;
    public Dictionary<ulong, string> reconnectPlayers;

    // public ServerGameSettings reconnectServerSettings;
    public string selectedCharacterName = "";
    private const string ongoingGameTitle = "You have a game in progress";
    private const string ongoingGameDescription = "Do you want to reconnect to the game?";
    private const string connectionTitle = "Error";
    private const string connectionDescription = "Your connection to the server has been lost.";
    private const string versionHashesTitle = "Warning";
    private const string versionHashesDescription =
        "Client and Server version hashes do not match.";
    WebSocket ws;

    private void Awake()
    {
        this.Init();
        this.clientId = ServerUtils.GetClientId();
    }

    public void Init()
    {
        this.serverIp = SelectServerIP.GetServerIp();
        this.serverName = SelectServerIP.GetServerName();

        if (Instance != null)
        {
            if (this.ws != null)
            {
                this.ws.Close();
            }

            ResetFields();
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void ResetFields()
    {
        this.LobbySession = "";
        this.GameSession = "";
        this.playerId = 0;
        this.serverTickRate_ms = 0;
        this.serverHash = "";
        this.playerCount = 0;
        this.gameStarted = false;
        this.clientId = "";
        this.simulatedPlayerCount = 0;
        this.lobbyCapacity = 0;
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        if (ws != null)
        {
            ws.DispatchMessageQueue();
        }
#endif
        if (this.gameStarted)
        {
            CancelInvoke("UpdateSimulatedCounter");
        }
    }

    public void JoinLobby()
    {
        // ValidateVersionHashes();
        ConnectToSession();
        InvokeRepeating("UpdateSimulatedCounter", 0, 1);
    }

    //     public void ConnectToLobby(string matchmaking_id)
    //     {
    //         // ValidateVersionHashes();
    //         ConnectToSession(matchmaking_id);
    //         LobbySession = matchmaking_id;
    //     }

    public void RefreshServerInfo()
    {
        this.serverIp = SelectServerIP.GetServerIp();
        this.serverName = SelectServerIP.GetServerName();
    }

    //     public void Reconnect()
    //     {
    //         this.reconnect = true;
    //         this.GameSession = this.reconnectGameId;
    //         this.playerId = this.reconnectPlayerId;
    //         this.serverSettings = this.reconnectServerSettings;
    //         this.serverTickRate_ms = (uint)this.serverSettings.RunnerConfig.ServerTickrateMs;
    //         this.serverHash = this.reconnectServerHash;
    //         this.playerCount = this.reconnectPlayerCount;
    //         this.gameStarted = true;
    //         this.playersIdName = GameServerConnectionManager.Instance.playersIdName;
    //     }

    class AcceptAllCertificates : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }

    private void ConnectToSession()
    {
        int hashCode = this.clientId.GetHashCode();
        ulong id = (ulong)(hashCode > 0 ? hashCode : hashCode * -1);
        SessionParameters.PlayerId = id;
        this.playerId = id;
        string url = makeWebsocketUrl("/play/" + id);
        ws = new WebSocket(url);
        ws.OnMessage += OnWebSocketMessage;
        ws.OnClose += OnWebsocketClose;
        ws.OnOpen += () =>
        {
            LobbySession = "sessionId";
        };
        ws.Connect();
    }

    private void OnWebSocketMessage(byte[] data)
    {
        try
        {
            GameState gameState = GameState.Parser.ParseFrom(data);
            if (!String.IsNullOrEmpty(gameState.GameId) && SessionParameters.GameId == null)
            {
                Debug.Log("no null " + gameState.GameId);
                SessionParameters.GameId = gameState.GameId;
                GameSession = gameState.GameId;
                Debug.Log("SessionParameters.GameId -> " + SessionParameters.GameId);
            }
            //     LobbyEvent lobbyEvent = LobbyEvent.Parser.ParseFrom(data);
            //     switch (lobbyEvent.Type)
            //     {
            //         case LobbyEventType.Connected:
            //             this.playerId = lobbyEvent.PlayerInfo.PlayerId;
            //             break;

            //         case LobbyEventType.PlayerAdded:
            //             lobbyEvent
            //                 .PlayersInfo
            //                 .ToList()
            //                 .ForEach(
            //                     playerInfo =>
            //                         this.playersIdName[playerInfo.PlayerId] = playerInfo.PlayerName
            //                 );
            //             break;

            //         case LobbyEventType.PreparingGame:
            //             GameSession = lobbyEvent.GameId;
            //             Debug.Log(lobbyEvent.GameConfig);
            //             engineServerSettings = lobbyEvent.GameConfig;
            //             // FIX THIS!!
            //             serverTickRate_ms = 30;
            //             serverHash = lobbyEvent.ServerHash;
            //             break;

            //         case LobbyEventType.NotifyPlayerAmount:
            //             this.playerCount = (int)lobbyEvent.AmountOfPlayers;
            //             this.lobbyCapacity = (int)lobbyEvent.Capacity;
            //             InvokeRepeating("UpdateSimulatedCounter", 0, 1);
            //             break;

            //         default:
            //             Debug.Log("Message received is: " + lobbyEvent.Type);
            //             break;
            //     }
            //     ;
        }
        catch (Exception e)
        {
            Debug.Log("InvalidProtocolBufferException: " + e);
        }
    }

    private void UpdateSimulatedCounter()
    {
        var limit = this.lobbyCapacity - this.simulatedPlayerCount;
        System.Random r = new System.Random();
        var randomNumber = r.Next(0, Math.Min(3, limit));
        this.simulatedPlayerCount = this.simulatedPlayerCount + randomNumber;
    }

    private void OnWebsocketClose(WebSocketCloseCode closeCode)
    {
        if (closeCode != WebSocketCloseCode.Normal)
        {
            Errors.Instance.HandleNetworkError(connectionTitle, connectionDescription);
        }
        else
        {
            Debug.Log("ServerConnection websocket closed normally");
        }
    }

    private string makeWebsocketUrl(string path)
    {
        if (serverIp.Contains("localhost"))
        {
            return "ws://" + serverIp + ":4000" + path;
        }
        else if (serverIp.Contains("10.150.20.186"))
        {
            return "ws://" + serverIp + ":4000" + path;
        }
        // Load test server
        else if (serverIp.Contains("168.119.71.104"))
        {
            return "ws://" + serverIp + ":4000" + path;
        }
        // Load test runner server
        else if (serverIp.Contains("176.9.26.172"))
        {
            return "ws://" + serverIp + ":4000" + path;
        }
        else
        {
            return "wss://" + serverIp + path;
        }
    }

    public void GetSelectedCharacter(AsyncOperation operation)
    {
        StartCoroutine(
            ServerUtils.GetSelectedCharacter(
                response =>
                {
                    ServerConnection.Instance.selectedCharacterName = response.selected_character;
                    if (operation != null)
                    {
                        operation.allowSceneActivation = true;
                    }
                },
                error =>
                {
                    Errors.Instance.HandleNetworkError("Error", error);
                }
            )
        );
    }
}
