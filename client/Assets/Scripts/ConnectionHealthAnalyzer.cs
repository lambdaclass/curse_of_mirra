using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConnectionHealthAnalyzer : MonoBehaviour
{
    long lastUpdateTimestamp;
    Queue<long> timestampDifferences = new Queue<long>();
    const int TIMESTAMP_DIFFERENCES_MAX_LENGTH = 5;
    const long SHOW_WARNING_THRESHOLD = 75;
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
            timestampDifferences.Dequeue();
        }
        timestampDifferences.Enqueue(timestampDifference);

        GameServerConnectionManager.Instance.currentPing = (uint)timestampDifferences.Peek();

        if(timestampDifferences.Count >= TIMESTAMP_DIFFERENCES_MAX_LENGTH)
        {
            if(timestampDifferences.Max() - timestampDifferences.Average() > SHOW_WARNING_THRESHOLD)
            {
                unstableConnection = true;
            }
            else
            {
                unstableConnection = false;
            }
        }
    }
}
