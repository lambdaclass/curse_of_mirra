using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ProtoBuf;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using NativeWebSocket;
// using WebSocketSharp;

public class LobbyConnection : MonoBehaviour
{
    [Tooltip("IP to connect to. If empty, localhost will be used")]
    public string server_ip = "localhost";
    public List<string> lobbiesList;
    public List<string> gamesList;
    public static LobbyConnection Instance;
    public string GameSession;
    public string LobbySession;
    public int playerId;
    public int playerCount;
    public bool gameStarted = false;

    WebSocket ws;

    public class Session
    {
        public string lobby_id { get; set; }
    }

    public class LobbiesResponse
    {
        public List<string> lobbies { get; set; }
    }

    public class GamesResponse
    {
        public List<string> current_games { get; set; }
    }

    public void CreateLobby()
    {
        StartCoroutine(GetRequest("http://" + server_ip + ":4000/new_lobby"));
    }

    public void ConnectToLobby(string matchmaking_id)
    {
        ConnectToSession(matchmaking_id);
        LobbySession = matchmaking_id;
    }

    private void Awake()
    {
        this.Init();
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
        this.playerId = -1;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        StartCoroutine(GetLobbies("http://" + server_ip + ":4000/current_lobbies"));
        StartCoroutine(GetGames("http://" + server_ip + ":4000/current_games"));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();
            string[] pages = uri.Split('/');
            int page = pages.Length - 1;
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    //Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    //Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Session session = JsonConvert.DeserializeObject<Session>(
                        webRequest.downloadHandler.text
                    );
                    Debug.Log("Creating and joining lobby ID: " + session.lobby_id);
                    LobbySession = session.lobby_id;
                    ConnectToSession(session.lobby_id);
                    break;
            }
        }
    }

    IEnumerator GetLobbies(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();
            string[] pages = uri.Split('/');
            int page = pages.Length - 1;
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    //Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    //Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    LobbiesResponse response = JsonConvert.DeserializeObject<LobbiesResponse>(
                        webRequest.downloadHandler.text
                    );
                    lobbiesList = response.lobbies;
                    break;
            }
        }
    }

    IEnumerator GetGames(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();
            string[] pages = uri.Split('/');
            int page = pages.Length - 1;
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    GamesResponse response = JsonConvert.DeserializeObject<GamesResponse>(
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
        ws = new WebSocket("ws://" + server_ip + ":4000/matchmaking/" + session_id);
        ws.OnMessage += OnWebSocketMessage;
        ws.OnError += (e) =>
        {
            // Debug.Log(
            //     "Error received from: " + ((WebSocket)sender).Url + ", Data: " + e.Exception.Message
            // );
            Debug.Log("Received Error");
        };
        ws.Connect();
    }

    
    void Update()
    {
      #if !UNITY_WEBGL || UNITY_EDITOR
      if (ws != null) {
        ws.DispatchMessageQueue();
      }
      #endif
    }

    private void OnWebSocketMessage(byte[] data)
    {
        LobbyEvent lobby_event = Serializer.Deserialize<LobbyEvent>((ReadOnlySpan<byte>)data);
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
                if (playerId == -1)
                {
                    playerId = (int)lobby_event.AddedPlayerId;
                }
                break;

            case LobbyEventType.PlayerCount:
                playerCount = (int)lobby_event.PlayerCount;
                break;

            case LobbyEventType.GameStarted:
                GameSession = lobby_event.GameId;
                break;

            default:
                Debug.Log("Message received is: " + lobby_event.Type);
                break;
        }
        ;
    }

    public void StartGame()
    {
        LobbyEvent lobbyEvent = new LobbyEvent { Type = LobbyEventType.StartGame };
        using (var stream = new MemoryStream())
        {
            Serializer.Serialize(stream, lobbyEvent);
            var msg = stream.ToArray();
            ws.Send(msg);
        }
        gameStarted = true;
    }
}
