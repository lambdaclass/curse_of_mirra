using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.Tools;
using Newtonsoft.Json;
using ProtoBuf;
using UnityEngine;
using UnityEngine.Networking;
using WebSocketSharp;
using static SocketConnectionManager;


public class LobbyConnection : MonoBehaviour
{
    [Tooltip("IP to connect to. If empty, localhost will be used")]
    public string server_ip = "localhost";

    [SerializeField]
    GameObject lobbyItemPrefab;

    [SerializeField]
    GameObject gameItemPrefab;

    [SerializeField]
    Transform lobbyListContainer;
    public List<GameObject> lobbiesList;

    [SerializeField]
    Transform gameListContainer;
    public List<GameObject> gamesList;

    public static LobbyConnection Instance;
    public string GameSession;
    public string LobbySession;
    public int playerId;
    public int playerCount;
    private bool gameStarted = false;

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
                    lobbiesList = response.lobbies
                        .Select(l =>
                        {
                            GameObject lobbyItem = Instantiate(lobbyItemPrefab, lobbyListContainer);
                            lobbyItem.GetComponent<LobbyItem>().setId(l);
                            return lobbyItem;
                        })
                        .ToList();

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
                    gamesList = response.current_games
                        .Select(l =>
                        {
                            GameObject gameItem = Instantiate(gameItemPrefab, gameListContainer);
                            gameItem.GetComponent<GameItem>().setId(l);
                            return gameItem;
                        })
                        .ToList();

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
        ws.OnError += (sender, e) =>
        {
            Debug.Log(
                "Error received from: " + ((WebSocket)sender).Url + ", Data: " + e.Exception.Message
            );
        };
        ws.Connect();
    }

    private void OnWebSocketMessage(object sender, MessageEventArgs e)
    {
        if (e.Data.Contains("GAME_ID"))
        {
            string game_id = e.Data.Split(": ")[1];
            print("The game id is: " + game_id);
            GameSession = game_id;
        }
        else if (e.Data.Contains("JOINED PLAYER"))
        {
            if (playerId == -1)
            {
                playerId = Int32.Parse(((e.Data).Split(": ")[1]));
            }
        }
        else if (e.Data.Contains("AMOUNT_OF_PLAYERS"))
        {
            playerCount = Int32.Parse(((e.Data).Split(": ")[1]));
        }
        else
        {
            Debug.Log("Message received is: " + e.Data);
        }
    }
    public void StartGame()
    {
        ws.Send("START_GAME");
        gameStarted = true;
    }

    private void Update()
    {
        if (!String.IsNullOrEmpty(GameSession) && gameStarted == false)
        {
            StartGame();
            MMSceneLoadingManager.LoadScene("BackendPlayground");
        }
    }
}
