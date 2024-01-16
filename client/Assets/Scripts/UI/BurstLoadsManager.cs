using System.Collections.Generic;
using Communication.Protobuf;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;

public class BurstLoadsManager : MonoBehaviour
{
    private const string BASIC_SKILL_KEY = "1";

    [SerializeField]
    private List<MMProgressBar> Bursts;
    private float SkillCooldown = 0f;

    void Start()
    {
        Bursts.ForEach(burst =>
        {
            burst.ForegroundBar.GetComponent<Image>().color = Utils.magenta;
        });
    }

    public void Update()
    {
        var player = Utils.GetGamePlayer(GameServerConnectionManager.Instance.playerId);

        UpdateCooldown(player);

        int basicBurstLoads = (int)player.AvailableBurstLoads[BASIC_SKILL_KEY];
        for (int i = 0; i < Bursts.Count; i++)
        {
            Image foregroundImage = Bursts[i].ForegroundBar.GetComponent<Image>();

            if (i <= basicBurstLoads - 1)
            {
                if (!foregroundImage.color.Equals(Utils.magenta))
                {
                    foregroundImage.color = Utils.magenta;
                }
            }
            else
            {
                foregroundImage.color = Utils.burstLoadsBarCharging;
            }

            UpdateBurstsBar(i, basicBurstLoads, player.BasicSkillCooldownLeft.Low);
        }
    }

    private void UpdateCooldown(OldPlayer player)
    {
        var currentCooldown = player.BasicSkillCooldownLeft.Low;
        if (SkillCooldown <= currentCooldown)
        {
            SkillCooldown = currentCooldown;
        }
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
        else
        {
            Bursts[currentIndex].UpdateBar01(0f);
        }
    }
}
