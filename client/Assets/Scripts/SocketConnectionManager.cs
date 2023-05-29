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
using System.Diagnostics;

public class SocketConnectionManager : MonoBehaviour
{
    public List<GameObject> players;
    public static List<GameObject> playersStatic;

    [Tooltip("Session ID to connect to. If empty, a new session will be created")]
    public string session_id = "";

    [Tooltip("IP to connect to. If empty, localhost will be used")]
    public string server_ip = "localhost";
    public static SocketConnectionManager Instance;
    public List<Player> gamePlayers;
    private int playerId;

    public static SocketConnectionManager instance;
    public uint currentPing;

    WebSocket ws;

    public class Session
    {
        public string session_id { get; set; }
    }

    public void Awake()
    {
        Instance = this;
        this.session_id = LobbyConnection.Instance.GameSession;
        this.server_ip = LobbyConnection.Instance.server_ip;
        playersStatic = this.players;
    }

    void Start()
    {
        playerId = LobbyConnection.Instance.playerId;

        if (this.session_id.IsNullOrEmpty())
        {
            StartCoroutine(GetRequest("http://" + server_ip + ":4000/new_session"));
        }
        else
        {
            ConnectToSession(this.session_id);
        }
    }
    Vector2 position = new Vector2(0, 0);
    Vector2 lastPosition = new Vector2(0, 0);

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
            print(
                "Error received from: " + ((WebSocket)sender).Url + ", Data: " + e.Exception.Message
            );
        };
        ws.Connect();
    }

    private void OnWebSocketMessage(object sender, MessageEventArgs e)
    {
        GameEvent game_event = Serializer.Deserialize<GameEvent>((ReadOnlySpan<byte>)e.RawData);
        switch (game_event.Type)
        {
            case GameEventType.StateUpdate:
                this.gamePlayers = game_event.Players;
                break;

            case GameEventType.PingUpdate:
                UInt64 currentPing = game_event.Latency;
                break;

            default:
                print("Message received is: " + game_event.Type);
                break;
        }
        ;
    }

    public void SendAction(ClientAction action)
    {
        using (var stream = new MemoryStream())
        {
            Serializer.Serialize(stream, action);
            var msg = stream.ToArray();
            ws.Send(msg);
        }
    }
}
