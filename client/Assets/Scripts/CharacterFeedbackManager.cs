using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MoreMountains.Tools;

// TODO: this could probably be a part of another class
public class CharacterFeedbackManager : MonoBehaviour
{
    [SerializeField]
    public UmaMarks umaMarks;

    public enum Marks
    {
        XandaMark = PlayerEffect.XandaMark,
        YugenMark = PlayerEffect.YugenMark,
        ElnarMark = PlayerEffect.ElnarMark,
    }

    private Queue<PlayerEffect> activeMarks = new Queue<PlayerEffect>();
    IDictionary<ulong, ISet<PlayerEffect>> playersMarks =
        new Dictionary<ulong, ISet<PlayerEffect>>();

    public void HandleUmaMarks(Player playerUpdate)
    {
        foreach (int effect in Enum.GetValues(typeof(Marks)))
        {
            if (playerUpdate.Effects.ContainsKey((ulong)effect))
            {
                DisplayEffectMark(playerUpdate.Id, GetEffect(effect));
            }
            else
            {
                RemoveMark(playerUpdate.Id, GetEffect(effect));
            }
        }
    }

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

    public void DisplayEffectMark(ulong playerId, PlayerEffect effect)
    {
        umaMarks.gameObject.SetActive(true);
        if (playersMarks.ContainsKey(playerId))
        {
            playersMarks[playerId].Add(effect);
        }
        else
        {
            playersMarks.Add(playerId, new HashSet<PlayerEffect>() { effect });
        }
        UpdateMarkImage(playersMarks[playerId].Count);
    }

    public void RemoveMark(ulong playerId, PlayerEffect effect)
    {
        if (playersMarks.ContainsKey(playerId) && playersMarks[playerId].Contains(effect))
        {
            playersMarks[playerId].Remove(effect);
            UpdateMarkImage(playersMarks[playerId].Count);
        }
    }

    private void UpdateMarkImage(int markCount)
    {
        umaMarks.SetImage(markCount);
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
        MMHealthBar healthBar = player.GetComponent<MMHealthBar>();

        switch (playerUpdate.Effects)
        {
            case var effect when effect.ContainsKey((ulong)PlayerEffect.Poisoned):
                if (!healthBar.ForegroundColor.Equals(MMColors.Green))
                {
                    ChangeHealthBarColor(healthBar, MMColors.Green);
                }
                break;
            default:
                if (!healthBar.ForegroundColor.Equals(MMColors.Red))
                {
                    ChangeHealthBarColor(healthBar, MMColors.BestRed);
                }
                break;
        }
    }
}
