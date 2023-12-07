using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class KillFeedManager : MonoBehaviour
{
    [SerializeField]
    KillFeedItem killFeedItem;

    [SerializeField]
    public List<CoMCharacter> charactersScriptableObjects;

    [SerializeField]
    public Sprite zoneIcon;

    public static KillFeedManager instance;
    private Queue<KillEvent> feedEvents = new Queue<KillEvent>();

    public ulong saveKillerId;
    public ulong myKillerId;

    public ulong playerToTrack;
    private const string ZONE_ID = "9999";

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

    Sprite GetUIIcon(ulong killerId)
    {
        if (killerId.ToString() == ZONE_ID)
        {
            return zoneIcon;
        }
        else
        {
            CoMCharacter characterIcon =
                KillFeedManager.instance.charactersScriptableObjects.Single(
                    characterSO =>
                        characterSO.name.Contains(Utils.GetCharacter(killerId).CharacterModel.name)
                );
            return characterIcon.UIIcon;
        }
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
            if (killEvent.Killed == SocketConnectionManager.Instance.playerId)
            {
                myKillerId = killEvent.KilledBy;
            }
            // TODO: fix this when the player names are fixed in the server.
            // string deathPlayerName = LobbyConnection.Instance.playersIdName[killEvent.Killed];
            // string killerPlayerName = LobbyConnection.Instance.playersIdName[killEvent.KilledBy];
            string deathPlayerName = killEvent.Killed.ToString();
            string killerPlayerName = killEvent.KilledBy.ToString();
            Sprite killerIcon = GetUIIcon(killEvent.KilledBy);
            Sprite killedIcon = GetUIIcon(killEvent.Killed);

            killFeedItem.SetPlayerData(killerPlayerName, killerIcon, deathPlayerName, killedIcon);
            GameObject item = Instantiate(killFeedItem.gameObject, transform);
            Destroy(item, 3.0f);
        }
    }
}
