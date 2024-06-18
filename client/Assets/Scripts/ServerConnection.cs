using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Google.Protobuf;
using NativeWebSocket;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

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

    public void JoinGame(string join_action)
    {
        // ValidateVersionHashes();
        ResetFields();
        ConnectToSession(join_action);
        // InvokeRepeating("UpdateSimulatedCounter", 0, 1);
    }

    //     public void ConnectToLobby(string matchmaking_id)
    //     {
    //         // ValidateVersionHashes();
    //         ConnectToSession(matchmaking_id);
    //         LobbySession = matchmaking_id;
    //     }

    public void LeaveLobby()
    {
        using (var stream = new MemoryStream())
        {
            var action = new LobbyEvent { Leave = new LeaveLobby { } };
            action.WriteTo(stream);
            var msg = stream.ToArray();
            ws.Send(msg);
        }
    }

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

    private void ConnectToSession(string join_action)
    {
        int hashCode = this.clientId.GetHashCode();
        ulong id = (ulong)(hashCode > 0 ? hashCode : hashCode * -1);
        SessionParameters.PlayerId = id;
        this.playerId = id;
        string character_name = CharactersManager.Instance.GoToCharacter.ToLower();
        string player_name = PlayerPrefs.GetString("playerName");
        string url = makeWebsocketUrl("/" + join_action + "/" + id + "/" + character_name + "/" + player_name);
        print(url);
        ws = ServerUtils.CreateWebSocket(url);
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
            LobbyEvent lobbyEvent = LobbyEvent.Parser.ParseFrom(data);
            switch (lobbyEvent.EventCase)
            {
                case LobbyEvent.EventOneofCase.Left:
                    SceneManager.LoadScene("MainScreen");
                    break;
                case LobbyEvent.EventOneofCase.Game:
                    GameState gameState = lobbyEvent.Game;
                    this.lobbyCapacity = 2;
                    if (!String.IsNullOrEmpty(gameState.GameId) && SessionParameters.GameId == null)
                    {
                        SessionParameters.GameId = gameState.GameId;
                        GameSession = gameState.GameId;
                    }
                    break;
            }
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
        CancelInvoke("UpdateSimulatedCounter");

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
        int port = 4000;

        if (serverIp.Contains("localhost"))
        {
            return "ws://" + serverIp + ":" + port + path;
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
