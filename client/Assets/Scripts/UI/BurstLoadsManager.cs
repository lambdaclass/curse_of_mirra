using System.Collections.Generic;
using DG.Tweening;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;

public class BurstLoadsManager : MonoBehaviour
{
    [SerializeField]
    private List<MMProgressBar> Bursts;
    private float SkillCooldown = 0f;

    void Start()
    {
        Bursts.ForEach(burst =>
        {
            burst.DelayedBarIncreasing.GetComponent<Image>().color = Utils.healthBarCyan;
        });
    }

    public void Update()
    {
        var player = Utils.GetGamePlayer(GameServerConnectionManager.Instance.playerId).Player;
        int avaibleStamina = (int)player.AvailableStamina;
        if(avaibleStamina <= (int)player.MaxStamina)
        {
            for (int currentLoad = 0; currentLoad < Bursts.Count; currentLoad++)
            {
                UpdateBurstsBar(currentLoad, avaibleStamina, player.StaminaInterval);
            }
        }
    }

    public void FinalBarColor(MMProgressBar currentBar){
        currentBar.DelayedBarIncreasing.GetComponent<Image>().color = Utils.healthBarCyan;
    }

    public void IncreasingBarColor(MMProgressBar currentBar){
        currentBar.DelayedBarIncreasing.GetComponent<Image>().color = Utils.burstLoadsBarCharging;
    }


    private void UpdateBurstsBar(int currentIndex, int burstLoads, ulong cooldownLeft)
    {

        if (currentIndex == burstLoads)
        {
            Bursts[currentIndex].UpdateBar01(1 - cooldownLeft / SkillCooldown);
        }
        else if (currentIndex < burstLoads)
        {
            Bursts[currentIndex].UpdateBar01(1f);
        }
    }
}
