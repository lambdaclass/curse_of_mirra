using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillFeedManager : MonoBehaviour
{
    [SerializeField] KillFeedItem killFeedItem;
    public bool kill = false;

    public void Init(string killerName, string killedName)
    {
        killFeedItem.SetPlayerNames(killerName, killedName);
        GameObject item = Instantiate(killFeedItem.gameObject, transform);
        Destroy(item, 1.5f);
        //TODO: Add create and destroy animations
    }

    void Update()
    {
        if (kill)
        {
            //TEST
            Init("player1", "player2");
        }
    }

}
