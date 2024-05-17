using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class LatencyAnalyzer : MonoBehaviour
{
    public bool showWarning, unstableConnection;
    int updateInterval = 50;
    protected int _timeLeftToUpdate;
    List<long> gameEventTimestamps = new List<long>();
    public static LatencyAnalyzer Instance;
    const int SPIKE_VALUE_THRESHOLD = 150;
    const int SPIKES_AMOUNT_THRESHOLD = 3;
    const int TIMESTAMPS_LIST_MAX_LENGTH = 100;
    const int SPIKES_UNTIL_WARNING = 1;
    const int MILLISECONDS_TO_WAIT = 3000;
    private const string CONNECTION_TITLE = "Error";
    private const string CONNECTION_DESCRIPTION = "Your connection to the server has been lost.";
    int amountOfSpikes = 0;
    private Entity gamePlayer;
    long currentTimeUnix, nextUpdateTimeUnix;

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

    // Start is called before the first frame update
    void Start()
    {
        _timeLeftToUpdate = updateInterval;
        ulong playerId = GameServerConnectionManager.Instance.playerId;
        Entity gamePlayer = Utils.GetGamePlayer(playerId);
        // Get the current Unix time in seconds
        currentTimeUnix = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        // Calculate the next update time by adding the update interval to the current time
        nextUpdateTimeUnix = currentTimeUnix + _timeLeftToUpdate;
    }

    // Update is called once per frame
    void Update()
    {
        currentTimeUnix = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        if (currentTimeUnix >= nextUpdateTimeUnix)
        {
            // Calculate the next update time
            nextUpdateTimeUnix = currentTimeUnix + _timeLeftToUpdate;
            long gameEventTimestamp = GameServerConnectionManager.Instance.gameEventTimestamp;
            long clientTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            if (gameEventTimestamp > 0 && (!GameServerConnectionManager.Instance.GameHasEnded()
                || (gamePlayer != null && gamePlayer.Player.Health != 0)))
            {
                long diffUpdateValue = clientTimestamp - gameEventTimestamp;

                // Redirect on disconnection
                if (diffUpdateValue >= (long)GameServerConnectionManager.Instance.maxMsBetweenEvents)
                {
                    DisconnectFeedback();
                    Errors.Instance.HandleNetworkError(CONNECTION_TITLE, CONNECTION_DESCRIPTION);
                }

                // Check if the list Length is already 10 and keep it that way
                if (gameEventTimestamps.Count == (int)GameServerConnectionManager.Instance.timestampsListMaxLength)
                {
                    gameEventTimestamps.RemoveAt(0);
                }
                else
                {
                    gameEventTimestamps.Add(gameEventTimestamp);
                }
                ConnectionStabilityCheck(gameEventTimestamps);
            }
        }
    }

    void ConnectionStabilityCheck(List<long> list)
    {
        int spikesCounter = 0;
        bool finishCounting = false;
        if (list.Count >= 2)
        {
            for (int i = 0; i < list.Count - 1; i++)
            {
                // Check for spikes
                if (list[i + 1] - list[i] >= (long)GameServerConnectionManager.Instance.spikeValueThreshold && !finishCounting)
                {
                    spikesCounter += 1;
                }
                int index = list.IndexOf(list[i + 1]);
                if (index == list.Count - 1)
                {
                    finishCounting = true;
                }
            }
        }

        if (finishCounting)
        {
            amountOfSpikes = spikesCounter;
        }
        showWarning = amountOfSpikes >= (long)GameServerConnectionManager.Instance.spikesUntilWarning;
        unstableConnection = amountOfSpikes >= (long)GameServerConnectionManager.Instance.spikesAmountThreshold;

    }
    public void DisconnectFeedback()
    {
        unstableConnection = false;
        Utils.BackToLobbyFromGame("MainScreen");
    }
}
