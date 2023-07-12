using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KillFeedItem : MonoBehaviour
{
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
        killerPlayer.text = "Player " + killer;
        killedPlayer.text = "Player " + killed;
    }
}
