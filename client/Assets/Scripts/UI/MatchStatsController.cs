using System;
using System.Linq;
using System.Timers;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;

public class MatchStatsController : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI alivePlayers;

    [SerializeField]
    TextMeshProUGUI zoneTimer;

    [SerializeField]
    TextMeshProUGUI killCount;

    ulong remainingSeconds = 1;

    void FixedUpdate()
    {
        alivePlayers.text = SocketConnectionManager.Instance.alivePlayers.Count().ToString();
        killCount.text = Utils
            .GetGamePlayer(SocketConnectionManager.Instance.playerId)
            ?.KillCount.ToString();

        if(remainingSeconds > 0) {
            EventsBuffer buffer = SocketConnectionManager.Instance.eventsBuffer;
            ulong elapsedTime = (ulong)buffer.lastEvent().ServerTimestamp - LobbyConnection.Instance.matchStartTime;
            ulong mapShrinkWaitMs = LobbyConnection
                .Instance
                .serverSettings
                .RunnerConfig
                .MapShrinkWaitMs;
            remainingSeconds = (mapShrinkWaitMs > elapsedTime) ?
                (mapShrinkWaitMs - elapsedTime) / 1000
                : 0;
            zoneTimer.text = remainingSeconds.ToString();
        }
    }
}
