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
    [Tooltip("IP to connect to. If empty, Brazil will be used")]
    public string serverName;
    public string serverIp;
    public List<string> lobbiesList;
    public List<string> gamesList;
    public static LobbyConnection Instance;
    public string GameSession;
    public string LobbySession;
    public ulong playerId;
    public bool isHost = false;
    public ulong hostId;
    public int playerCount;
    public Dictionary<ulong, string> playersIdName = new Dictionary<ulong, string>();
    public uint serverTickRate_ms;
    public string serverHash;
    public ServerGameSettings serverSettings;

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
    public ServerGameSettings reconnectServerSettings;

    private const string ongoingGameTitle = "You have a game in progress";
    private const string ongoingGameDescription = "Do you want to reconnect to the game?";
    private const string connectionTitle = "Error";
    private const string connectionDescription = "Your connection to the server has been lost.";
    private const string versionHashesTitle = "Warning";
    private const string versionHashesDescription =
        "Client and Server version hashes do not match.";

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
        public string server_version;
    }

    [Serializable]
    public class GamesResponse
    {
        public List<string> current_games;
    }

    [Serializable]
    public class CurrentGameResponse
    {
        public bool ongoing_game;
        public bool on_character_selection;
        public int player_count;
        public string server_hash;
        public string current_game_id;
        public ulong current_game_player_id;
        public List<Player> players;
        public Configs game_config;

        [Serializable]
        public class Player
        {
            public ulong id;
            public string character_name;
            public string player_name;
        }

        [Serializable]
        public class Configs
        {
            public string runner_config;
            public string character_config;
            public string skills_config;
        }
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
        LoadClientId();
        MaybeReconnect();
        PopulateLists();
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

            Destroy(gameObject);
            return;
        }
        Instance = this;
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

    private void LoadClientId()
    {
        if (!PlayerPrefs.HasKey("client_id"))
        {
            Guid g = Guid.NewGuid();
            PlayerPrefs.SetString("client_id", g.ToString());
        }

        this.clientId = PlayerPrefs.GetString("client_id");
    }

    private void MaybeReconnect()
    {
        StartCoroutine(GetCurrentGame());
    }

    public void CreateLobby()
    {
        ValidateVersionHashes();
        StartCoroutine(GetRequest(makeUrl("/new_lobby")));
    }

    public void ConnectToLobby(string matchmaking_id)
    {
        ValidateVersionHashes();
        ConnectToSession(matchmaking_id);
        LobbySession = matchmaking_id;
    }

    private void ValidateVersionHashes()
    {
        if (serverHash.Trim() != GitInfo.GetGitHash().Trim())
        {
            Errors.Instance.HandleVersionHashesError(versionHashesTitle, versionHashesDescription);
        }
    }

    public void Refresh()
    {
        this.serverIp = SelectServerIP.GetServerIp();
        this.serverName = SelectServerIP.GetServerName();
        PopulateLists();
        MaybeReconnect();
    }

    public void QuickGame()
    {
        ValidateVersionHashes();
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

    public void Reconnect()
    {
        this.reconnect = true;
        this.GameSession = this.reconnectGameId;
        this.playerId = this.reconnectPlayerId;
        this.serverSettings = this.reconnectServerSettings;
        this.serverTickRate_ms = (uint)this.serverSettings.RunnerConfig.ServerTickrateMs;
        this.serverHash = this.reconnectServerHash;
        this.playerCount = this.reconnectPlayerCount;
        this.gameStarted = true;
        this.playersIdName = SocketConnectionManager.Instance.playersIdName;
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
                case UnityWebRequest.Result.Success:
                    Session session = JsonUtility.FromJson<Session>(
                        webRequest.downloadHandler.text
                    );
                    ConnectToSession(session.lobby_id);
                    break;
                default:
                    Errors.Instance.HandleNetworkError(connectionTitle, connectionDescription);
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
                case UnityWebRequest.Result.Success:
                    var response = JsonUtility.FromJson<LobbiesResponse>(
                        webRequest.downloadHandler.text
                    );
                    lobbiesList = response.lobbies;
                    serverHash = response.server_version;
                    break;
                default:
                    Errors.Instance.HandleNetworkError(connectionTitle, connectionDescription);
                    break;
            }
        }
    }

    IEnumerator GetGames()
    {
        string url = makeUrl("/current_games");
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
                    Errors.Instance.HandleNetworkError(connectionTitle, connectionDescription);
                    break;
            }
        }
    }

    IEnumerator GetCurrentGame()
    {
        string url = makeUrl("/player_game/" + this.clientId);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.certificateHandler = new AcceptAllCertificates();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    CurrentGameResponse response = JsonUtility.FromJson<CurrentGameResponse>(
                        webRequest.downloadHandler.text
                    );

                    if (response.ongoing_game)
                    {
                        this.reconnectPossible = true;
                        this.reconnectToCharacterSelection = response.on_character_selection;
                        this.reconnectPlayerCount = response.player_count;
                        this.reconnectGameId = response.current_game_id;
                        this.reconnectPlayerId = response.current_game_player_id;
                        this.reconnectPlayerCount = response.player_count;
                        this.reconnectServerHash = response.server_hash;

                        this.reconnectPlayers = new Dictionary<ulong, string>();
                        response.players.ForEach(
                            player => this.reconnectPlayers.Add(player.id, player.character_name)
                        );

                        this.reconnectServerSettings = parseReconnectServerSettings(
                            response.game_config
                        );
                        Errors.Instance.HandleReconnectError(
                            ongoingGameTitle,
                            ongoingGameDescription
                        );
                    }
                    break;
                default:
                    break;
            }
        }
    }

    private void ConnectToSession(string sessionId)
    {
        var player_name = PlayerPrefs.GetString("playerName");
        string url = makeWebsocketUrl("/matchmaking/" + sessionId + "/" + player_name);
        ws = new WebSocket(url);
        ws.OnMessage += OnWebSocketMessage;
        ws.OnClose += OnWebsocketClose;
        ws.OnOpen += () =>
        {
            LobbySession = sessionId;
        };
        ws.Connect();
    }

    private void OnWebSocketMessage(byte[] data)
    {
        try
        {
            LobbyEvent lobbyEvent = LobbyEvent.Parser.ParseFrom(data);
            switch (lobbyEvent.Type)
            {
                case LobbyEventType.Connected:
                    this.playerId = lobbyEvent.PlayerInfo.PlayerId;
                    break;

                case LobbyEventType.PlayerAdded:
                    this.hostId = lobbyEvent.HostPlayerId;
                    this.isHost = this.playerId == this.hostId;
                    this.playerCount = lobbyEvent.PlayersInfo.Count();
                    lobbyEvent.PlayersInfo
                        .ToList()
                        .ForEach(
                            playerInfo =>
                                this.playersIdName[playerInfo.PlayerId] = playerInfo.PlayerName
                        );
                    break;

                case LobbyEventType.PlayerRemoved:
                    this.playerCount = lobbyEvent.PlayersInfo.Count();
                    this.hostId = lobbyEvent.HostPlayerId;
                    this.isHost = this.playerId == this.hostId;
                    this.playersIdName.Remove(lobbyEvent.RemovedPlayerInfo.PlayerId);
                    break;

                case LobbyEventType.GameStarted:
                    GameSession = lobbyEvent.GameId;
                    serverSettings = lobbyEvent.GameConfig;
                    serverTickRate_ms = (uint)serverSettings.RunnerConfig.ServerTickrateMs;
                    serverHash = lobbyEvent.ServerHash;
                    gameStarted = true;
                    break;

                default:
                    Debug.Log("Message received is: " + lobbyEvent.Type);
                    break;
            }
            ;
        }
        catch (Exception e)
        {
            Debug.Log("InvalidProtocolBufferException: " + e);
        }
    }

    private void OnWebsocketClose(WebSocketCloseCode closeCode)
    {
        if (closeCode != WebSocketCloseCode.Normal)
        {
            Errors.Instance.HandleNetworkError(connectionTitle, connectionDescription);
            UnityEngine.SceneManagement.SceneManager.LoadScene("Lobbies");
        }
    }

    private string makeUrl(string path)
    {
        if (serverIp.Contains("localhost"))
        {
            return "http://" + serverIp + ":4000" + path;
        }
        else if (serverIp.Contains("10.150.20.186"))
        {
            return "http://" + serverIp + ":4000" + path;
        }
        else
        {
            return "https://" + serverIp + path;
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
        else
        {
            return "wss://" + serverIp + path;
        }
    }

    public bool isConnectionOpen()
    {
        return ws.State == NativeWebSocket.WebSocketState.Open;
    }

    private ServerGameSettings parseReconnectServerSettings(CurrentGameResponse.Configs configs)
    {
        JsonParser parser = new JsonParser(new JsonParser.Settings(100000)); //GameSettings

        RunnerConfig parsedRunner = parser.Parse<RunnerConfig>(
            configs.runner_config.TrimStart('\uFEFF')
        );
        CharacterConfig characters = parser.Parse<CharacterConfig>(
            configs.character_config.TrimStart('\uFEFF')
        );
        SkillsConfig skills = parser.Parse<SkillsConfig>(configs.skills_config.TrimStart('\uFEFF'));

        return new ServerGameSettings
        {
            RunnerConfig = parsedRunner,
            CharacterConfig = characters,
            SkillsConfig = skills,
        };
    }
}
