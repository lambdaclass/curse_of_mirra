using System;
using System.Collections.Generic;
using System.Linq;

public class EventsBuffer
{
    const int bufferLimit = 30;
    public List<GameState> updatesBuffer = new List<GameState>();

    public Dictionary<ulong, long> lastTimestampsSeen = new Dictionary<ulong, long>();

    public long firstTimestamp = 0;

    public long deltaInterpolationTime { get; set; }

    public void AddEvent(GameState newEvent)
    {
        if (updatesBuffer.Count == bufferLimit)
        {
            updatesBuffer.RemoveAt(0);
        }
        updatesBuffer.Add(newEvent);
    }

    public int Count()
    {
        return updatesBuffer.Count;
    }

    public GameState lastEvent()
    {
        int lastIndex = updatesBuffer.Count - 1;
        return updatesBuffer[lastIndex];
    }

    public Tuple<GameState, int> getNextEventToRender(long pastTime)
    {
        GameState nextGameEvent = null;
        int nextGameEventIndex = 0;
        for (int i = 0; i < updatesBuffer.Count; i++)
        {
            GameState update = updatesBuffer[i];
            if (update.ServerTimestamp > pastTime)
            {
                if (nextGameEvent == null || update.ServerTimestamp < nextGameEvent.ServerTimestamp)
                {
                    nextGameEvent = update;
                    nextGameEventIndex = i;
                }
            }
        }

        if (nextGameEvent == null)
        {
            return new Tuple<GameState, int>(this.lastEvent(), updatesBuffer.Count - 1);
        }
        else
        {
            return new Tuple<GameState, int>(nextGameEvent, nextGameEventIndex);
        }
    }

    // /*
    // This function is used to tell whether if another player is moving between the
    // previous, current and following events to render, if true, we will show the walking
    // animation. The previous rendered event is 30ms in the past, the current is the
    // event we're going to render now and the following is the next we're going to render
    // in the next 30ms.
    // After getting all those events, we just check that the amount of moving states
    // which the player has, is greater or equal than one, assuming that he was moving, is moving now or he will.
    // */
    public bool playerIsMoving(ulong playerId, long pastTime)
    {
        Tuple<GameState, int> currentEventToRender = this.getNextEventToRender(pastTime);
        var index = currentEventToRender.Item2;
        int previousIndex;
        int nextIndex;

        if (index == 0)
        {
            previousIndex = 0;
        }
        else
        {
            previousIndex = index - 1;
        }

        if (index == (updatesBuffer.Count - 1))
        {
            nextIndex = updatesBuffer.Count - 1;
        }
        else
        {
            nextIndex = index + 1;
        }

        GameState previousRenderedEvent = updatesBuffer[previousIndex];

        // There are a few frames during which this is outdated and produces an error
        if (
            previousRenderedEvent.Players.Count
            == GameServerConnectionManager.Instance.gamePlayers.Count
        )
        {
            Entity serverPlayerUpdate = new Entity(previousRenderedEvent.Players[playerId]);
            if (serverPlayerUpdate.IsMoving)
            {
                return true;
            }
            serverPlayerUpdate = new Entity(currentEventToRender.Item1.Players[playerId]);
            if (serverPlayerUpdate.IsMoving)
            {
                return true;
            }

            GameState followingEventToRender = updatesBuffer[nextIndex];
            serverPlayerUpdate = new Entity(followingEventToRender.Players[playerId]);
            if (serverPlayerUpdate.IsMoving)
            {
                return true;
            }
        }

        return false;
    }

    public void setLastTimestampSeen(ulong playerId, long serverTimestamp)
    {
        lastTimestampsSeen[playerId] = serverTimestamp;
    }

    public bool timestampAlreadySeen(ulong playerId, long serverTimestamp)
    {
        if (!lastTimestampsSeen.ContainsKey(playerId))
        {
            return false;
        }
        return lastTimestampsSeen[playerId] == serverTimestamp;
    }

    private List<Entity> ConvertToList(ICollection<Entity> collection)
    {
        return new List<Entity>(collection);
    }
}
