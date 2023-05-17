using WebSocketSharp;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;
using System.Net;
using System;
using System.Xml.Linq;
using ProtoBuf;
using MoreMountains.TopDownEngine;
using System.Diagnostics;

public class SocketConnectionManager : MonoBehaviour
{
    public LevelManager levelManager;
    public CinemachineCameraController camera;
    public Character prefab;
    public List<GameObject> players;
    public Queue<PositionUpdate> positionUpdates = new Queue<PositionUpdate>();

    [Tooltip("Session ID to connect to. If empty, a new session will be created")]
    public string session_id = "";

    [Tooltip("IP to connect to. If empty, localhost will be used")]
    public string server_ip = "localhost";

    WebSocket ws;

    private int totalPlayers;
    private int playerCount = 0;
    private int playerId;
    public static SocketConnectionManager Instance;
    public uint currentPing;


    public class GameResponse
    {
        public List<Player> players { get; set; }
    }

    public class Session
    {
        public string session_id { get; set; }
    }

    public struct PositionUpdate
    {
        public long x;
        public long y;
        public int player_id;
    }

    public class Position
    {
        public long x { get; set; }
        public long y { get; set; }
    }

    public class Player
    {
        public int id { get; set; }
        public int health { get; set; }
        public Position position { get; set; }
    }

    public void Awake()
    {
        this.session_id = LobbyConnection.Instance.GameSession;
        this.totalPlayers = LobbyConnection.Instance.playerCount;
        Instance = this;
    }
    public void GeneratePlayer()
    {
        for (int i = 0; i < totalPlayers; i++)
        {
            Character newPlayer = Instantiate(prefab, levelManager.InitialSpawnPoint.transform.position, Quaternion.identity);
            newPlayer.name = "Player" + " " + (i + 1);
            newPlayer.PlayerID = (i + 1).ToString();

            players.Add(newPlayer.gameObject);
            levelManager.Players.Add(newPlayer);
        }
        levelManager.PlayerPrefabs = (levelManager.Players).ToArray();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Send the player's action every 30 ms approximately.
        GeneratePlayer();
        playerId = LobbyConnection.Instance.playerId;
        setCameraToPlayer(LobbyConnection.Instance.playerId);
        float tickRate = 1f / 30f;
        InvokeRepeating("sendAction", tickRate, tickRate);
        InvokeRepeating("updatePing", 0.5f, 0.5f);

        if (this.session_id.IsNullOrEmpty())
        {
            StartCoroutine(GetRequest("http://" + server_ip + ":4000/new_session"));
        }
        else
        {
            ConnectToSession(this.session_id);
        }
    }

    void updatePing()
    {
        var timer = new Stopwatch();
        timer.Start();
        if (ws.Ping())
        {
            timer.Stop();
            TimeSpan timetaken = timer.Elapsed;
            currentPing = (uint) timetaken.Milliseconds;
        };
    }

    void sendAction()
    {
        if (ws == null)
        {
            return;
        }
        if (Input.GetKey(KeyCode.W))
        {
            ClientAction action = new ClientAction { Action = Action.Move, Direction = Direction.Up };
            SendAction(action);
        }
        if (Input.GetKey(KeyCode.A))
        {
            ClientAction action = new ClientAction { Action = Action.Move, Direction = Direction.Left };
            SendAction(action);
        }
        if (Input.GetKey(KeyCode.D))
        {
            ClientAction action = new ClientAction { Action = Action.Move, Direction = Direction.Right };
            SendAction(action);
        }
        if (Input.GetKey(KeyCode.S))
        {
            ClientAction action = new ClientAction { Action = Action.Move, Direction = Direction.Down };
            SendAction(action);
        }
        if (Input.GetKey(KeyCode.E))
        {
            ClientAction action = new ClientAction { Action = Action.AttackAoe };
            SendAction(action);
        }

        //ClientAction ping_action = new ClientAction { Action = Action.UpdatePing };
        //SendAction(ping_action);
    }

    private void setCameraToPlayer(int playerID)
    {
        //print(levelManager.PlayerPrefabs.Length);
        foreach (Character player in levelManager.PlayerPrefabs)
        {
            if (Int32.Parse(player.PlayerID) == playerID)
            {
                this.camera.SetTarget(player);
                this.camera.StartFollowing();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        while (positionUpdates.TryDequeue(out var positionUpdate))
        {
            this.players[positionUpdate.player_id].transform.position = new Vector3(positionUpdate.x / 10f - 50.0f, this.players[positionUpdate.player_id].transform.position.y, positionUpdate.y / 10f + 50.0f);
        }
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
                    Session session = JsonConvert.DeserializeObject<Session>(webRequest.downloadHandler.text);
                    print("Creating and joining Session ID: " + session.session_id);
                    ConnectToSession(session.session_id);
                    break;

            }
        }
    }

    private void ConnectToSession(string session_id)
    {
        print("ws://" + server_ip + ":4000/play/" + session_id + "/" + playerId);
        ws = new WebSocket("ws://" + server_ip + ":4000/play/" + session_id + "/" + playerId);
        ws.OnMessage += OnWebSocketMessage;
        ws.OnError += (sender, e) =>
        {
            print("Error received from: " + ((WebSocket)sender).Url + ", Data: " + e.Exception.Message);
        };
        ws.Connect();
    }

    private void OnWebSocketMessage(object sender, MessageEventArgs e)
    {
        // Debug.Log("Message received from: " + ((WebSocket)sender).Url + ", Data: " + e.Data);
        if (e.Data == "OK" || e.Data.Contains("CONNECTED_TO"))
        {
            //Debug.Log("Nothing to do");
        }
        else if (e.Data.Contains("ERROR"))
        {
            print("Error message: " + e.Data);
        }
        else
        {
            try
            {
                UpdatePing ping_update = Serializer.Deserialize<UpdatePing>((ReadOnlySpan<byte>)e.RawData);
                currentPing = ping_update.Latency;
            }
            catch (System.Exception)
            {
                GameStateUpdate game_update = Serializer.Deserialize<GameStateUpdate>((ReadOnlySpan<byte>)e.RawData);
                for (int i = 0; i < game_update.Players.Count; i++)
                {
                    var player = this.players[i];
                    var new_position = game_update.Players[i].Position;
                    print(game_update.Players[i]);
                    positionUpdates.Enqueue(new PositionUpdate { x = new_position.Y, y = -new_position.X, player_id = i });
                }
            }
        }
    }

    private void SendAction(ClientAction action)
    {
        using (var stream = new MemoryStream())
        {
            Serializer.Serialize(stream, action);
            var msg = stream.ToArray();
            ws.Send(msg);
        }
    }
}
