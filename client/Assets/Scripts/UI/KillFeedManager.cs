using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillFeedManager : MonoBehaviour
{
    [SerializeField]
    KillFeedItem killFeedItem;

    public static KillFeedManager instance;
    private Queue<KillEvent> feedEvents = new Queue<KillEvent>();

    public void Awake()
    {
        KillFeedManager.instance = this;
    }

    public void putEvents(List<KillEvent> feedEvent)
    {
        feedEvent.ForEach((killEvent) => feedEvents.Enqueue(killEvent));
    }

    public void Update()
    {
        KillEvent killEvent;
        while (feedEvents.TryDequeue(out killEvent))
        {
            killFeedItem.SetPlayerNames(killEvent.KilledBy.ToString(), killEvent.Killed.ToString());
            GameObject item = Instantiate(killFeedItem.gameObject, transform);
            Destroy(item, 3.0f);
        }
    }
}
