using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConnectionHealthAnalyzer : MonoBehaviour
{
    long lastUpdateTimestamp;
    List<long> timestampDifferences = new List<long>();
    const int TIMESTAMP_DIFFERENCES_TO_CHECK_WARNING = 5;
    const int TIMESTAMP_DIFFERENCES_MAX_LENGTH = 30;
    const long SHOW_WARNING_THRESHOLD = 75;
    const long STOP_WARNING_THRESHOLD = 40;
    const long MS_WITHOUT_UPDATE_SHOW_WARNING = 3000;
    const long MS_WITHOUT_UPDATE_DISCONNECT = 10000;
    public static bool unstableConnection = false;

    void Start()
    {
        GameServerConnectionManager.OnGameEventTimestampChanged += OnGameEventTimestampChanged;
    }

    void Update()
    {
        if(lastUpdateTimestamp > 0)
        {
            long msSinceLastUpdate = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - lastUpdateTimestamp;
            
            if (!unstableConnection && msSinceLastUpdate > MS_WITHOUT_UPDATE_SHOW_WARNING)
            {
                unstableConnection = true;
            }

            if(msSinceLastUpdate > MS_WITHOUT_UPDATE_DISCONNECT)
            {
                Disconnect();
            }
        }
    }

    private void OnGameEventTimestampChanged(long newTimestamp)
    {
        if(lastUpdateTimestamp == 0)
        {
            lastUpdateTimestamp = newTimestamp;
        }

        long timestampDifference = newTimestamp - lastUpdateTimestamp;
        lastUpdateTimestamp = newTimestamp;

        if(timestampDifferences.Count > TIMESTAMP_DIFFERENCES_MAX_LENGTH)
        {
            timestampDifferences.RemoveAt(0);
        }
        timestampDifferences.Add(timestampDifference);

        if(timestampDifferences.Count >= TIMESTAMP_DIFFERENCES_TO_CHECK_WARNING)
        {
            if(timestampDifferences.Max() - timestampDifferences.Take(TIMESTAMP_DIFFERENCES_TO_CHECK_WARNING).Average() > SHOW_WARNING_THRESHOLD)
            {
                unstableConnection = true;
            }
            else if(timestampDifferences.Max() - timestampDifferences.Average() < STOP_WARNING_THRESHOLD)
            {
                unstableConnection = false;
            }
        }
    }

    private void Disconnect()
    {
        unstableConnection = false;
        Utils.BackToLobbyFromGame("MainScreen");
        Errors.Instance.HandleNetworkError("Error", "Your connection to the server has been lost.");
    }
}
