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
    private float nextActionTime = 0.0f;
    public float period = 1f;

    public float time = 0f;

    ulong seconds = 0;

    void Awake()
    {
        seconds = LobbyConnection.Instance.serverSettings.RunnerConfig.MapShrinkWaitMs / 1000;
    }

    void Start()
    {
        zoneTimer.text = seconds.ToString();
    }

    void FixedUpdate()
    {
        alivePlayers.text = SocketConnectionManager.Instance.alivePlayers.Count().ToString();
        killCount.text = Utils
            .GetGamePlayer(SocketConnectionManager.Instance.playerId)
            ?.KillCount.ToString();

        ulong timeRemainingMilliseconds = (ulong)(LobbyConnection.Instance.serverSettings.RunnerConfig.MapShrinkWaitMs - (DateTime.Now - LobbyConnection.Instance.matchStarted).TotalMilliseconds);
        zoneTimer.text = (timeRemainingMilliseconds / 1000).ToString();
    }
}
