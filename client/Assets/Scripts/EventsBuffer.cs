using System.Collections.Generic;
using UnityEngine;

public class EventsBuffer : MonoBehaviour
{
  const int bufferLimit = 30;
  public List<GameEvent> updatesBuffer = new List<GameEvent>();

  public long firstTimestamp;

  public float deltaInterpolationTime {get; set;}

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

  public GameEvent getNextEventToRender(float pastTime){
    List<GameEvent> nextGameEvents = updatesBuffer.FindAll(ge => ge.ServerTimestamp > pastTime);
    if(nextGameEvents.Count == 0){
        return this.lastEvent();
    }
    return nextGameEvents[0];
  }
}
