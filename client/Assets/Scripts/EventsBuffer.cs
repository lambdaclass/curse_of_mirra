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
}
