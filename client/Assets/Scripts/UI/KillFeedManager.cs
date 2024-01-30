using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KillFeedManager : MonoBehaviour
{
    [SerializeField]
    KillFeedItem killFeedItem;

    [SerializeField]
    public List<CoMCharacter> charactersScriptableObjects;

    [SerializeField]
    public Sprite zoneIcon;

    public static KillFeedManager instance;
    private Queue<KillEntry> feedEvents = new Queue<KillEntry>();

    public ulong saveKillerId;
    public ulong myKillerId;

    public ulong playerToTrack;
    private const string ZONE_ID = "9999";

    public void Awake()
    {
        KillFeedManager.instance = this;
        playerToTrack = GameServerConnectionManager.Instance.playerId;
    }

    public void putEvents(List<KillEntry> newFeedEvent)
    {
        newFeedEvent.ForEach((killEvent) => feedEvents.Enqueue(killEvent));
    }

    public ulong GetKiller(ulong deathPlayerId)
    {
        ulong killerId = 0;
        for (int i = 0; i < feedEvents.Count; i++)
        {
            if (feedEvents.ElementAt(i).VictimId == deathPlayerId)
                killerId = feedEvents.ElementAt(i).KillerId;
        }
        return killerId;
    }

    Sprite GetUIIcon(ulong killerId)
    {
        if (killerId.ToString() == ZONE_ID)
        {
            return zoneIcon;
        }
        else
        {
            CoMCharacter characterIcon = KillFeedManager
                .instance
                .charactersScriptableObjects
                .Single(
                    characterSO =>
                        characterSO.name.Contains(Utils.GetCharacter(killerId).CharacterModel.name)
                );
            return characterIcon.UIIcon;
        }
    }

    public void Update()
    {
        KillEntry killEvent;
        while (feedEvents.TryDequeue(out killEvent))
        {
            Debug.Log("new kill event" + killEvent);
            if (playerToTrack == killEvent.VictimId)
            {
                saveKillerId = killEvent.KillerId;
                playerToTrack = saveKillerId;
            }
            if (killEvent.VictimId == GameServerConnectionManager.Instance.playerId)
            {
                myKillerId = killEvent.KillerId;
            }
            // TODO: fix this when the player names are fixed in the server.
            // string deathPlayerName = ServerConnection.Instance.playersIdName[killEvent.VictimId];
            // string killerPlayerName = ServerConnection.Instance.playersIdName[killEvent.KillerId];
            string deathPlayerName = killEvent.VictimId.ToString();
            string killerPlayerName = killEvent.KillerId.ToString();
            Sprite killerIcon = GetUIIcon(killEvent.KillerId);
            Sprite killedIcon = GetUIIcon(killEvent.VictimId);

            killFeedItem.SetPlayerData(killerPlayerName, killerIcon, deathPlayerName, killedIcon);
            GameObject item = Instantiate(killFeedItem.gameObject, transform);
            Destroy(item, 3.0f);
        }
    }
}
