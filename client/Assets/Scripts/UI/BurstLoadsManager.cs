using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;

public class BurstLoadsManager : MonoBehaviour
{
    [SerializeField]
    private List<MMProgressBar> Bursts;

    void Awake() {
        Bursts.ForEach(burst =>
        {
            burst.ForegroundBar.GetComponent<Image>().color = Utils.magenta;
        });     
    }

    public void Update()
    {
        var player = Utils.GetGamePlayer(SocketConnectionManager.Instance.playerId);
        Debug.Log(player);
        int burstLoads = (int)player.AvailableBurstLoads;
        for (int i = 0; i < Bursts.Count; i++)
        {
            Color burstBarColor = Bursts[i].ForegroundBar.GetComponent<Image>().color;
            if (i <= burstLoads-1)
            {
                if (!burstBarColor.Equals(Utils.magenta)){
                    Bursts[i].ForegroundBar.GetComponent<Image>().color = Utils.magenta;
                }
            }
            else
            {
                Bursts[i].ForegroundBar.GetComponent<Image>().color = Utils.burstLoadsBarCharging;
            }

            if (i == burstLoads)
            {
                Bursts[i].UpdateBar01(1-player.BasicSkillCooldownLeft.Low / 5000f);
            } else if (i < burstLoads)
            {
                Bursts[i].UpdateBar01(1f);
            } else
            {
                Bursts[i].UpdateBar01(0f);
            }
        }
    }
}
