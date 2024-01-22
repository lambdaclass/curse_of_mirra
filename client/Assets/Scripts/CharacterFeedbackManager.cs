using System;
using Communication.Protobuf;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;

public class CharacterFeedbackManager : MonoBehaviour
{
    private enum StateEffects
    {
        Slowed = PlayerEffect.Slowed,
        Paralyzed = PlayerEffect.Paralyzed,
        Poisoned = PlayerEffect.Poisoned,
        OutOfArea = PlayerEffect.OutOfArea
    }

    private CustomCharacter character;

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

    void Start()
    {
        character = this.GetComponentInParent<CustomCharacter>();
    }

    private bool PlayerShouldSeeEffectMark(OldPlayer playerUpdate, PlayerEffect effect)
    {
        ulong attackerId = GetEffectCauser(playerUpdate, effect);
        return playerUpdate.Id == GameServerConnectionManager.Instance.playerId
            || attackerId == GameServerConnectionManager.Instance.playerId;
    }

    private ulong GetEffectCauser(OldPlayer playerUpdate, PlayerEffect effect)
    {
        return playerUpdate.Effects[(ulong)effect].CausedBy;
    }

    public void ChangeHealthBarColor(MMHealthBar healthBar, Color color)
    {
        healthBar.ForegroundColor = Utils.GetHealthBarGradient(color);
    }

    public void ToggleHealthBar(GameObject player, OldPlayer playerUpdate)
    {
        var healthBarFront = player
            .GetComponent<CustomCharacter>()
            .GetComponent<MMHealthBar>()
            .TargetProgressBar
            .ForegroundBar
            .GetComponent<Image>();
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
            if (playerUpdate.Id == GameServerConnectionManager.Instance.playerId)
            {
                healthBarFront.color = Utils.healthBarCyan;
            }
            else
            {
                healthBarFront.color = Utils.healthBarRed;
            }
        }
    }

    public void ManageStateFeedbacks(OldPlayer playerUpdate, StateManagerUI stateManagerUI)
    {
        ManageFeedbacks(stateManagerUI, playerUpdate);
        ToggleHealthBar(character.gameObject, playerUpdate);
    }

    private void ManageFeedbacks(StateManagerUI stateManagerUI, OldPlayer playerUpdate)
    {
        if (playerUpdate.Effects.Count > 0)
        {
            foreach (int effect in Enum.GetValues(typeof(StateEffects)))
            {
                string name = Enum.GetName(typeof(StateEffects), effect);
                bool hasEffect = playerUpdate.Effects.ContainsKey((ulong)effect);
                stateManagerUI.ToggleState(name, playerUpdate.Id, hasEffect);
                character.GetComponent<CharacterFeedbacks>().SetActiveFeedback(name, hasEffect);
            }
        }
    }
}
