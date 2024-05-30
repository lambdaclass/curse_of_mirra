using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConnectionHealthAnalyzer : MonoBehaviour
{
    long lastUpdateTimestamp;
    List<long> timestampDifferences = new List<long>();
    public static bool unstableConnection = false;
    bool matchFinished = false;

    void Start()
    {
        GameServerConnectionManager.OnGameEventTimestampChanged += OnGameEventTimestampChanged;
        GameServerConnectionManager.OnMatchFinished += OnMatchFinished;
    }

    void Update()
    {
        if(!matchFinished && lastUpdateTimestamp > 0)
        {
            long msSinceLastUpdate = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - lastUpdateTimestamp;
            
            if (!unstableConnection && msSinceLastUpdate > GameServerConnectionManager.Instance.msWithoutUpdateShowWarning)
            {
                unstableConnection = true;
            }

            if(msSinceLastUpdate > GameServerConnectionManager.Instance.msWithoutUpdateDisconnect)
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

        if(timestampDifferences.Count > GameServerConnectionManager.Instance.timestampDifferencesSamplesMaxLength)
        {
            timestampDifferences.RemoveAt(0);
        }
        timestampDifferences.Add(timestampDifference);

        GameServerConnectionManager.Instance.currentPing = (uint)timestampDifferences.Last();

        if(timestampDifferences.Count >= GameServerConnectionManager.Instance.timestampDifferenceSamplesToCheckWarning)
        {
            if(timestampDifferences.Max() - timestampDifferences.Take(GameServerConnectionManager.Instance.timestampDifferenceSamplesToCheckWarning).Average() > GameServerConnectionManager.Instance.showWarningThreshold)
            {
                unstableConnection = true;
            }
            else if(timestampDifferences.Max() - timestampDifferences.Average() < GameServerConnectionManager.Instance.stopWarningThreshold)
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

    private void OnMatchFinished()
    {
        GameServerConnectionManager.OnGameEventTimestampChanged -= OnGameEventTimestampChanged;
        matchFinished = true;
    }
}
