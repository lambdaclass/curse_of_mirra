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
    public float period = 1f;

    public float time = 0f;

    ulong seconds = 0;

    void Awake()
    {
        seconds = 0;
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

        time += Time.deltaTime;

        if (time >= period && seconds > 0)
        {
            time = time - period;
            seconds--;
            zoneTimer.text = seconds.ToString();
        }
    }
}
