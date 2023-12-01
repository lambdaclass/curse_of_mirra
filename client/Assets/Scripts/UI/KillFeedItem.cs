using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KillFeedItem : MonoBehaviour
{
    private const string ZONE_ID = "9999";
    private const string LOOT_ID = "1111";

    [SerializeField]
    TextMeshProUGUI killerPlayer;

    [SerializeField]
    TextMeshProUGUI killedPlayer;

    void Start()
    {
        GetComponent<Animator>().Play("KillfeedAnim", 0, 0.0f);
    }

    public void SetPlayerNames(string killer, string killed)
    {
        if (killer == ZONE_ID)
        {
            killerPlayer.text = "Zone";
            killedPlayer.text = "Player " + killed;
            return;
        }
        if (killed == LOOT_ID)
        {
            killerPlayer.text = "Loot";
            killedPlayer.text = "Player " + killed;
            return;
        }
        killerPlayer.text = "Player " + killer;
        killedPlayer.text = "Player " + killed;
    }
}
