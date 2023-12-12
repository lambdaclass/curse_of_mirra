using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MoreMountains.Tools;
using UnityEngine.UI;

public class CharacterFeedbackManager : MonoBehaviour
{
    private PlayerEffect GetEffect(int effectNumber)
    {
        PlayerEffect effectFound = PlayerEffect.None;
        foreach (PlayerEffect effect in Enum.GetValues(typeof(PlayerEffect)))
        {
            if (effectNumber == (int)effect)
            {
                effectFound = effect;
            }
        }

        return effectFound;
    }

    private bool PlayerShouldSeeEffectMark(Player playerUpdate, PlayerEffect effect)
    {
        ulong attackerId = GetEffectCauser(playerUpdate, effect);
        return playerUpdate.Id == SocketConnectionManager.Instance.playerId
            || attackerId == SocketConnectionManager.Instance.playerId;
    }

    private ulong GetEffectCauser(Player playerUpdate, PlayerEffect effect)
    {
        return playerUpdate.Effects[(ulong)effect].CausedBy;
    }

    public void ChangeHealthBarColor(MMHealthBar healthBar, Color color)
    {
        healthBar.ForegroundColor = Utils.GetHealthBarGradient(color);
    }

    public void ToggleHealthBar(GameObject player, Player playerUpdate)
    {
        var healthBarFront = player
            .GetComponent<CustomCharacter>()
            .GetComponent<MMHealthBar>()
            .TargetProgressBar.ForegroundBar.GetComponent<Image>();
        if (
            playerUpdate.Effects.ContainsKey((ulong)PlayerEffect.Poisoned)
            && !healthBarFront.color.Equals(Utils.healthBarPoisoned)
        )
        {
            healthBarFront.color = Utils.healthBarPoisoned;
        }
        if (
            !playerUpdate.Effects.ContainsKey((ulong)PlayerEffect.Poisoned)
            && healthBarFront.color.Equals(Utils.healthBarPoisoned)
        )
        {
            if (playerUpdate.Id == SocketConnectionManager.Instance.playerId)
            {
                healthBarFront.color = Utils.healthBarCyan;
            }
            else
            {
                healthBarFront.color = Utils.healthBarRed;
            }
        }
    }
}
