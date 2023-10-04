using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class KillFeedManager : MonoBehaviour
{
    [SerializeField]
    KillFeedItem killFeedItem;

    public static KillFeedManager instance;
    private Queue<KillEvent> feedEvents = new Queue<KillEvent>();

    public ulong saveKillerId;

    public ulong playerToTrack;

    public void Awake()
    {
        KillFeedManager.instance = this;
        playerToTrack = SocketConnectionManager.Instance.playerId;
    }

    public void putEvents(List<KillEvent> feedEvent)
    {
        feedEvent.ForEach((killEvent) => feedEvents.Enqueue(killEvent));
    }

    public ulong GetKiller(ulong deathPlayerId)
    {
        ulong killerId = 0;
        for (int i = 0; i < feedEvents.Count; i++)
        {
            if (feedEvents.ElementAt(i).Killed == deathPlayerId)
                killerId = feedEvents.ElementAt(i).KilledBy;
        }
        return killerId;
    }

    public void Update()
    {
        KillEvent killEvent;
        while (feedEvents.TryDequeue(out killEvent))
        {
            if (playerToTrack == killEvent.Killed)
            {
                saveKillerId = killEvent.KilledBy;
                playerToTrack = saveKillerId;
            }
            killFeedItem.SetPlayerNames(killEvent.KilledBy.ToString(), killEvent.Killed.ToString());
            GameObject item = Instantiate(killFeedItem.gameObject, transform);
            Destroy(item, 3.0f);
        }
    }
}
