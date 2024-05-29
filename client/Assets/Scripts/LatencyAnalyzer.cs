using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class LatencyAnalyzer : MonoBehaviour
{
    public bool showUnstableConnectionWarning, showDisconnectedWarning, unstableConnection;
    long lastUpdateTimestamp;
    Queue<long> timestampsDifferences = new Queue<long>();
    public static LatencyAnalyzer Instance;
    const int SPIKE_VALUE_THRESHOLD = 150;
    const int SPIKES_AMOUNT_THRESHOLD = 3;
    const int TIMESTAMPS_LIST_MAX_LENGTH = 100;
    const int SPIKES_UNTIL_WARNING = 1;
    const int NO_SERVER_UPDATE_WARNING_MS = 3000;
    const int NO_SERVER_UPDATE_DISCONNECT_MS = 7500;
    private const string CONNECTION_TITLE = "Error";
    private const string CONNECTION_DESCRIPTION = "Your connection to the server has been lost.";
    int amountOfSpikes = 0;

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

    void Start()
    {
        ulong playerId = GameServerConnectionManager.Instance.playerId;
        Entity gamePlayer = Utils.GetGamePlayer(playerId);
        GameServerConnectionManager.OnGameEventTimestampChanged += OnGameEventTimestampChanged;
    }

    void Update()
    {
        if(lastUpdateTimestamp > 0)
        {
            long msSinceLastUpdate = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - lastUpdateTimestamp;
            if(msSinceLastUpdate > NO_SERVER_UPDATE_WARNING_MS)
            {
                showDisconnectedWarning = true;

                if(msSinceLastUpdate > NO_SERVER_UPDATE_DISCONNECT_MS)
                {
                    DisconnectFeedback();
                    Errors.Instance.HandleNetworkError(CONNECTION_TITLE, CONNECTION_DESCRIPTION);
                }
            }
            else
            {
                showDisconnectedWarning = false;
            }
        }
    }

    private void OnGameEventTimestampChanged(long newTimestamp)
    {
        if(lastUpdateTimestamp == 0)
        {
            lastUpdateTimestamp = newTimestamp;
        }

        long msSinceLastUpdate = newTimestamp - lastUpdateTimestamp;
        lastUpdateTimestamp = newTimestamp;

        if (timestampsDifferences.Count == (int)GameServerConnectionManager.Instance.timestampsListMaxLength)
        {
            timestampsDifferences.Dequeue();
        }
        timestampsDifferences.Enqueue(msSinceLastUpdate);

        if((timestampsDifferences.Max() - timestampsDifferences.Min()) > (long)GameServerConnectionManager.Instance.spikeValueThreshold)
        {
            showUnstableConnectionWarning = true;
        }
        else
        {
            showUnstableConnectionWarning = false;
        }
    }

    public void DisconnectFeedback()
    {
        unstableConnection = false;
        Utils.BackToLobbyFromGame("MainScreen");
    }
}

