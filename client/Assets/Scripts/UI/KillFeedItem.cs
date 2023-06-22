using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KillFeedItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI killerPlayer;
    [SerializeField] TextMeshProUGUI killedPlayer;

    public void SetPlayerNames(string killer, string killed)
    {
        killerPlayer.text = killer;
        killedPlayer.text = killed;
    }
}
