using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class CrateItem : MonoBehaviour
{
    public ulong serverId;

    [SerializeField]
    Animator animator;
    Health health;
    private Dictionary<string, int> animationHashes;

    void Awake()
    {
        animationHashes = new Dictionary<string, int>
        {
            { "Shake", Animator.StringToHash("Shake") },
            { "Open", Animator.StringToHash("Open") }
        };
        health = GetComponent<Health>();
    }

    public void Initialize(Entity item)
    {
        serverId = item.Id;

        var position = Utils.transformBackendOldPositionToFrontendPosition(item.Position);
        position.y = 0;
        transform.position = position;

        health = GetComponent<Health>();
        health.CurrentHealth = item.Crate.Health;
        health.InitialHealth = item.Crate.Health;
        health.MaximumHealth = item.Crate.Health;
        gameObject.SetActive(true);
    }

    public void UpdateHealth(ulong updatedHealth)
    {
        if (updatedHealth != health.CurrentHealth)
        {
            health.SetHealth(updatedHealth);
            ExecuteHitFeedback();
        }
    }

    public void ExecuteHitFeedback()
    {
        PlayAnimation("Shake");
    }

    public void ExecuteOpenedFeedback()
    {
        health.SetHealth(0);
        PlayAnimation("Open");
    }

    public void PlayAnimation(string animationName)
    {
        if (animationHashes.TryGetValue(animationName, out int hash))
        {
            animator.SetTrigger(hash);
        }
    }
}
