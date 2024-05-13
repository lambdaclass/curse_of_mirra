using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class LatencyAnalyzer : MonoBehaviour
{
    public bool unstableConnection, disconnect;
    public long pingValue;
    /// the frequency at which the PING counter should update
    float updateInterval = .5f;
    protected float _timeLeftToUpdate;
    int spikeValueThreshold = 1500;
    int spikesAmountThreshold = 3;
    List<long> gameEventTimestamps = new List<long>();
    const int TIMESTAMPS_LIST_MAX_LENGTH = 10;
    public static LatencyAnalyzer Instance;
    public int secondsToWaitForReconnect = 3000;
    private const string connectionTitle = "Error";
    private const string connectionDescription = "Your connection to the server has been lost.";

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        _timeLeftToUpdate = updateInterval;     
    }

    // Update is called once per frame
    void Update()
    {
        long gameEventTimestamp = GameServerConnectionManager.Instance.gameEventTimestamp;
        _timeLeftToUpdate = _timeLeftToUpdate - Time.deltaTime;
        if(gameEventTimestamp > 0 && !GameServerConnectionManager.Instance.GameHasEnded())
        {
            long clientTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            // Show connection icon to warn client
            if(clientTimestamp - gameEventTimestamp >= 2000)
            {
                disconnect = true;
            }
            else
            {
                disconnect = false;
            }
            // Redirect on disconnect
            if(clientTimestamp - gameEventTimestamp >= secondsToWaitForReconnect)
            {
                //DisconnectFeedback();
                //Errors.Instance.HandleNetworkError(connectionTitle, connectionDescription);
            }
        }
        if (_timeLeftToUpdate <= 0.0)
        {
            _timeLeftToUpdate = updateInterval;
            
            pingValue = gameEventTimestamp;

            // Check if the list Length is already 10 and keep it that way
            if( gameEventTimestamps.Count >= TIMESTAMPS_LIST_MAX_LENGTH){
                gameEventTimestamps.RemoveAt(0);
            }
            gameEventTimestamps.Add(gameEventTimestamp);

            // Check if the target value exists in the list
            unstableConnection = ConnectionStabilityCheck(gameEventTimestamps);
            if(unstableConnection){
                //print("More spikes than usual found in list: " + gameEventTimestamps);
            }
        }
    }

    bool ConnectionStabilityCheck(List<long> list)
    {
        int spikesCounter = 0;
        if(list.Count >= 2){
            for(int i = 0; i < list.Count - 1; i++){
                // Check for spikes
                print(list[i + 1] - list[i]);
                if (list[i + 1] - list[i] >= spikeValueThreshold)
                {
                    spikesCounter += 1;
                }
            }
        }
        //print(spikesCounter);
        return spikesCounter >=  spikesAmountThreshold;
    }
    public void DisconnectFeedback()
    {
        disconnect = false;
        Utils.BackToLobbyFromGame("MainScreen");
    }
}
