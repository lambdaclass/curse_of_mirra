using System.Collections.Generic;
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
            burst.ForegroundBar.GetComponent<Image>().color = Utils.healthBarCyan;
        });
    }

    public void Update()
    {
        var player = Utils.GetGamePlayer(GameServerConnectionManager.Instance.playerId).Player;

        int basicBurstLoads = (int)player.AvailableStamina;
        for (int i = 0; i < Bursts.Count; i++)
        {
            Image foregroundImage = Bursts[i].ForegroundBar.GetComponent<Image>();

            if (i <= (basicBurstLoads - 1))
            {
                if (!foregroundImage.color.Equals(Utils.healthBarCyan))
                {
                    foregroundImage.color = Utils.healthBarCyan;
                }
            }
            else
            {
                foregroundImage.color = Utils.burstLoadsBarCharging;
            }

            UpdateBurstsBar(i, basicBurstLoads, player.StaminaInterval);
        }
    }

    private void UpdateCooldown(Entity entity)
    {
        var currentCooldown = entity.Player.StaminaInterval;
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
