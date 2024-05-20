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

    private ulong saveKillerId;
    private ulong myKillerId;

    private ulong currentTrackedPlayer;
    private bool currentTrackedPlayerIsSet = false;
    private const ulong ZONE_ID = 9999;

    void Awake()
    {
        KillFeedManager.instance = this;
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
        if (killerId == ZONE_ID)
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
        if (GameServerConnectionManager.Instance.gamePlayers?.Count() > 0 && currentTrackedPlayerIsSet == false)
        {
            currentTrackedPlayer = GameServerConnectionManager.Instance.playerId;
            currentTrackedPlayerIsSet = true;
        }

        KillEntry killEvent = null;
        while (feedEvents.TryDequeue(out killEvent))
        {
            if (currentTrackedPlayer == killEvent.VictimId)
            {
                saveKillerId = killEvent.KillerId;
                currentTrackedPlayer = saveKillerId;
            }

            if (killEvent.VictimId == GameServerConnectionManager.Instance.playerId)
            {
                myKillerId = killEvent.KillerId;
            }
            // TODO: fix this when the player names are fixed in the server.
            // string deathPlayerName = ServerConnection.Instance.playersIdName[killEvent.VictimId];
            // string killerPlayerName = ServerConnection.Instance.playersIdName[killEvent.KillerId];
            ulong deathPlayerId = killEvent.VictimId;
            ulong killerPlayerId = killEvent.KillerId;

            string deathPlayerName = Utils.GetGamePlayer(deathPlayerId).Name;
            string killerPlayerName;
            if (killerPlayerId == ZONE_ID)
            {
                killerPlayerName = ZONE_ID.ToString();
            }
            else
            {
                killerPlayerName = Utils.GetGamePlayer(killerPlayerId).Name;

            }
            Sprite killerIcon = GetUIIcon(killEvent.KillerId);
            Sprite killedIcon = GetUIIcon(killEvent.VictimId);

            killFeedItem.SetPlayerData(killerPlayerName, killerIcon, deathPlayerName, killedIcon);
            GameObject item = Instantiate(killFeedItem.gameObject, transform);
            Destroy(item, 3.0f);
        }

        if (Utils.GetGamePlayer(currentTrackedPlayer)?.Player.Health <= 0 && killEvent == null)
        {
            currentTrackedPlayer = ZONE_ID;
        }
    }

    public ulong GetSaveKillderId()
    {
        return this.saveKillerId;
    }

    public void SetSaveKillderId(ulong newSaveKillderId)
    {
        this.saveKillerId = newSaveKillderId;
    }

    public ulong GetMyKillerId()
    {
        return this.myKillerId;
    }

    public ulong GetCurrentTrackedPlayer()
    {
        return this.currentTrackedPlayer;
    }

    public void SetCurrentTrackedPlayer(ulong newCurrentTrackedPlayer)
    {
        this.currentTrackedPlayer = newCurrentTrackedPlayer;
    }
}
