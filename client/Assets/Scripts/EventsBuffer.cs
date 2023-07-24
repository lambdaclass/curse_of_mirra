using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventsBuffer
{
    const int bufferLimit = 30;
    public List<GameEvent> updatesBuffer = new List<GameEvent>();

    public long firstTimestamp = 0;

    public long deltaInterpolationTime { get; set; }

    public void AddEvent(GameEvent newEvent)
    {
        if (updatesBuffer.Count == bufferLimit)
        {
            updatesBuffer.RemoveAt(0);
        }
        updatesBuffer.Add(newEvent);
    }

    public GameEvent lastEvent()
    {
        int lastIndex = updatesBuffer.Count - 1;
        return updatesBuffer[lastIndex];
    }

    public GameEvent getNextEventToRender(long pastTime)
    {
        GameEvent nextGameEvent = updatesBuffer
            .Where(ge => ge.ServerTimestamp > pastTime)
            .OrderBy(ge => ge.ServerTimestamp)
            .FirstOrDefault();

        if (nextGameEvent == null)
        {
            return this.lastEvent();
        }
        else
        {
            return nextGameEvent;
        }
    }

    public bool playerIsMoving(ulong playerId, long pastTime)
    {
        var count = 0;
        GameEvent previousRenderedEvent = this.getNextEventToRender(pastTime - 30);
        GameEvent currentEventToRender = this.getNextEventToRender(pastTime);
        GameEvent followingEventToRender = this.getNextEventToRender(pastTime + 30);

        count +=
            (previousRenderedEvent.Players.ToList().Find(p => p.Id == playerId)).Action
            == PlayerAction.Moving
                ? 1
                : 0;
        count +=
            (currentEventToRender.Players.ToList().Find(p => p.Id == playerId)).Action
            == PlayerAction.Moving
                ? 1
                : 0;
        count +=
            (followingEventToRender.Players.ToList().Find(p => p.Id == playerId)).Action
            == PlayerAction.Moving
                ? 1
                : 0;

        Debug.Log("the count is: " + count);
        return count >= 2;
    }
}
