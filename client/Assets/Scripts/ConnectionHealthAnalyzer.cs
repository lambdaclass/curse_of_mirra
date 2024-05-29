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
    public static bool unstableConnection = false;

    void Start()
    {
        GameServerConnectionManager.OnGameEventTimestampChanged += OnGameEventTimestampChanged;
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

        GameServerConnectionManager.Instance.currentPing = (uint)timestampDifferences.Last();

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
}
