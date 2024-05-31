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

        // We don't do anything until we have a full time window sample.
        if(timestampDifferences.Count < GameServerConnectionManager.Instance.timestampDifferenceSamplesToCheckWarning)
        {
            return;
        }

        if(SpikeDetected(timestampDifferences))
        {
            unstableConnection = true;
        }
        else if(IsConnectionStable(timestampDifferences))
        {
            unstableConnection = false;
        };
    }

    private bool SpikeDetected(List<long> timestampDifferences)
    {
        // We take a subsample of the whole list to check spikes.
        var spikeSample = timestampDifferences.Take(GameServerConnectionManager.Instance.timestampDifferenceSamplesToCheckWarning);
        return spikeSample.Max() - spikeSample.Average() > GameServerConnectionManager.Instance.showWarningThreshold;
    }

    private bool IsConnectionStable(List<long> timestampDifferences)
    {
        return timestampDifferences.Max() - timestampDifferences.Average() < GameServerConnectionManager.Instance.stopWarningThreshold;
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
