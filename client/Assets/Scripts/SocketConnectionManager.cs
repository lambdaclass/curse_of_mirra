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
using MoreMountains.Tools;

public class SocketConnectionManager : MonoBehaviour
{
    [SerializeField] MMTouchJoystick joystick;
    public LevelManager levelManager;
    public CinemachineCameraController camera;
    public Character prefab;
    public List<GameObject> players;
    public Queue<PlayerUpdate> playerUpdates = new Queue<PlayerUpdate>();

    [Tooltip("Session ID to connect to. If empty, a new session will be created")]
    public string session_id = "";

    [Tooltip("IP to connect to. If empty, localhost will be used")]
    public string server_ip = "localhost";

    WebSocket ws;

    private int totalPlayers;
    private int playerCount = 0;
    private int playerId;

    public class GameResponse
    {
        public List<Player> players { get; set; }
    }

    public class Session
    {
        public string session_id { get; set; }
    }

    public struct PlayerUpdate
    {
        public long x;
        public long y;
        public int player_id;
        public long health;
        public PlayerAction action;
    }

    public class Position
    {
        public long x { get; set; }
        public long y { get; set; }
    }

    public enum PlayerAction
    {
        Nothing = 0,
        Attacking = 1,
    }

    public class Player
    {
        public int id { get; set; }
        public int health { get; set; }
        public Position position { get; set; }
        public PlayerAction action { get; set; }
    }

    public static SocketConnectionManager Instance;

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

    public void Awake()
    {
        this.Init();
        Instance = this;
        DontDestroyOnLoad(gameObject);
        this.session_id = LobbyConnection.Instance.GameSession;
        this.totalPlayers = LobbyConnection.Instance.playerCount;
    }
    public void GeneratePlayer()
    {
        for (int i = 0; i < totalPlayers; i++)
        {
            if (LobbyConnection.Instance.playerId == i + 1)
            {
                // Player1 is the ID to match with the client InputManager
                prefab.PlayerID = "Player1";
            }
            else
            {
                prefab.PlayerID = "";
            }
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

        if (this.session_id.IsNullOrEmpty())
        {
            StartCoroutine(GetRequest("http://" + server_ip + ":4000/new_session"));
        }
        else
        {
            ConnectToSession(this.session_id);
        }
    }

    void sendAction()
    {
        Vector2 position = new Vector2(0, 0);

        if (joystick.isActiveAndEnabled)
        {
            position = new Vector2(joystick.RawValue.x, joystick.RawValue.y);
            print("joystick: " + joystick.RawValue);
            print("position: " + position);
            //ClientAction action = new ClientAction { Action = Action.Move, Direction = Vector2 {x:0, y:1} };
            //SendAction(action);
        }
        else
        {
            // new Vector2 JOYSTICK (0,0)
            /* if (Input.GetKey(KeyCode.W))
            {
                // joystick.RawValue replace its values with positive o negative 1 according to the direction of each letter
                //ClientAction action = new ClientAction { Action = Action.Move, Direction = Vector2 {x:0, y:1} };
                //SendAction(action);
            } */
        }

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
        if (Input.GetKeyDown(KeyCode.J))
        {
            // This sends the action
            ClientAction action = new ClientAction { Action = Action.Attack, Direction = Direction.Down };
            SendAction(action);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            ClientAction action = new ClientAction { Action = Action.Attack, Direction = Direction.Up };
            SendAction(action);

        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            ClientAction action = new ClientAction { Action = Action.Attack, Direction = Direction.Right };
            SendAction(action);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            ClientAction action = new ClientAction { Action = Action.Attack, Direction = Direction.Left };
            SendAction(action);
        }
    }

    public void sendAttackAoe(Vector2 buttonValues)
    {
        print(buttonValues);
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
        while (playerUpdates.TryDequeue(out var playerUpdate))
        {

            this.players[playerUpdate.player_id].transform.position = new Vector3(playerUpdate.x / 10f - 50.0f, this.players[playerUpdate.player_id].transform.position.y, playerUpdate.y / 10f + 50.0f);

            Health healthComponent = this.players[playerUpdate.player_id].GetComponent<Health>();
            healthComponent.SetHealth(playerUpdate.health);

            bool isAttacking = playerUpdate.action == PlayerAction.Attacking;
            this.players[playerUpdate.player_id].GetComponent<AttackController>().SwordAttack(isAttacking);
            if (isAttacking)
            {
                print("attack");
            }
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
                    Debug.Log("Creating and joi ning Session ID: " + session.session_id);
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
            //Debug.Log("Error received from: " + ((WebSocket)sender).Url + ", Data: " + e.Exception.Message);
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
            //Debug.Log("Error message: " + e.Data);
        }
        else
        {
            GameStateUpdate game_update = Serializer.Deserialize<GameStateUpdate>((ReadOnlySpan<byte>)e.RawData);
            for (int i = 0; i < game_update.Players.Count; i++)
            {
                var player = this.players[i];
                var new_position = game_update.Players[i].Position;

                playerUpdates.Enqueue(
                    new PlayerUpdate
                    {
                        x = ((long)new_position.Y),
                        y = -((long)new_position.X),
                        player_id = i,
                        health = game_update.Players[i].Health,
                        action = (PlayerAction)game_update.Players[i].Action,
                    }
                );

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
