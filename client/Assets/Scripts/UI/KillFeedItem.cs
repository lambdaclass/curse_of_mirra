using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KillFeedItem : MonoBehaviour
{
    private const string ZONE_ID = "9999";
    private const string LOOT_ID = "1111";

    [SerializeField]
    TextMeshProUGUI killerPlayer,
        killedPlayer;

    [SerializeField]
    Image killedImage,
        killerImage;

    void Start()
    {
        GetComponent<Animator>().Play("KillfeedAnim", 0, 0.0f);
    }

    public void SetPlayerData(string killer, Sprite killerIcon, string killed, Sprite killedIcon)
    {
        if (killer == ZONE_ID)
        {
            killerPlayer.text = "Zone";
            killerImage.sprite = killerIcon;
            killedPlayer.text = killed;
            killedImage.sprite = killedIcon;
            return;
        }
        if (killed == LOOT_ID)
        {
            killerPlayer.text = "Loot";
            killedPlayer.text = killed;
            killedImage.sprite = killedIcon;
            return;
        }
        killerPlayer.text = killer;
        killerImage.sprite = killerIcon;
        killedPlayer.text = killed;
        killedImage.sprite = killedIcon;
    }
}
