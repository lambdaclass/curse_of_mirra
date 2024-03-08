using System;
using System.Linq;
using System.Timers;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatchStatsController : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI alivePlayers;

    [SerializeField]
    TextMeshProUGUI zoneTimer;

    [SerializeField]
    TextMeshProUGUI killCount;

    [SerializeField] Image zoneTimerImage;

    [SerializeField]
    Sprite shrinkingSprite;

    [SerializeField]
    Sprite waitingSprite;
    public float period = 1f;

    public float time = 0f;

    ulong seconds = 0;
    bool isShrinking = false;

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
        if(isShrinking != GameServerConnectionManager.Instance.shrinking){
            zoneTimerImage.sprite = isShrinking ? shrinkingSprite : waitingSprite;
            isShrinking = GameServerConnectionManager.Instance.shrinking;
        }

        if (GameServerConnectionManager.Instance.gamePlayers != null)
        {
            alivePlayers.text = GameServerConnectionManager
                .Instance
                .gamePlayers
                .Sum(playerEntity => Convert.ToInt32(playerEntity.Player.Health > 0))
                .ToString();
        }

        killCount.text = Utils
            .GetGamePlayer(GameServerConnectionManager.Instance.playerId)
            ?.Player
            .KillCount
            .ToString();

        var time = GameServerConnectionManager.Instance.zoneShrinkTime / 1000;

        zoneTimer.text = time.ToString();
    }
}
