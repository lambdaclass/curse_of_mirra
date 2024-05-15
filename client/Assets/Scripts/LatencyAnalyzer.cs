using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class LatencyAnalyzer : MonoBehaviour
{
    public bool showWarning, unstableConnection;
    float updateInterval = .05f;
    protected float _timeLeftToUpdate;
    List<long> gameEventTimestamps = new List<long>();
    public static LatencyAnalyzer Instance;
    const int SPIKE_VALUE_THRESHOLD = 150;
    const int SPIKES_AMOUNT_THRESHOLD = 3;
    const int TIMESTAMPS_LIST_MAX_LENGTH = 100;
    const int MILLISECONDS_UNTIL_WARNING = 2000;
    const int MILLISECONDS_TO_WAIT = 3000;
    private const string CONNECTION_TITLE = "Error";
    private const string CONNECTION_DESCRIPTION = "Your connection to the server has been lost.";

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
        long clientTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        _timeLeftToUpdate = _timeLeftToUpdate - Time.deltaTime;
        if (gameEventTimestamp > 0 && !GameServerConnectionManager.Instance.GameHasEnded())
        {
            long diffUpdateValue = clientTimestamp - gameEventTimestamp;

            // Block actions
            unstableConnection = diffUpdateValue >= (long)GameServerConnectionManager.Instance.msUntilWarning;

            // Redirect on disconnection 
            if (diffUpdateValue >= (long)GameServerConnectionManager.Instance.maxMsBetweenEvents)
            {
                DisconnectFeedback();
                Errors.Instance.HandleNetworkError(CONNECTION_TITLE, CONNECTION_DESCRIPTION);
            }
        }
        if (_timeLeftToUpdate <= 0.0)
        {
            _timeLeftToUpdate = updateInterval;

            // Check if the list Length is already 10 and keep it that way
            if (gameEventTimestamps.Count >= (long)GameServerConnectionManager.Instance.timestampsListMaxLength)
            {
                gameEventTimestamps.RemoveAt(0);
            }
            gameEventTimestamps.Add(gameEventTimestamp);

            ConnectionStabilityCheck(gameEventTimestamps);
        }
    }

    void ConnectionStabilityCheck(List<long> list)
    {
        int spikesCounter = 0;
        if (list.Count >= 2)
        {
            for (int i = 0; i < list.Count - 1; i++)
            {
                // Check for spikes
                if (list[i + 1] - list[i] >= (long)GameServerConnectionManager.Instance.spikeValueThreshold)
                {
                    spikesCounter += 1;
                }
            }
        }
        showWarning = spikesCounter >= 1;
        unstableConnection = spikesCounter >= (long)GameServerConnectionManager.Instance.spikesAmountThreshold;
    }
    public void DisconnectFeedback()
    {
        unstableConnection = false;
        Utils.BackToLobbyFromGame("MainScreen");
    }
}
