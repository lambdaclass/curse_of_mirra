using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.VFX;

public class CharacterBase : MonoBehaviour
{
    [SerializeField]
    public GameObject PlayerName,
        Hitbox,
        FeedbackContainer,
        AimDirection,
        SkillRange,
        spawnFeedback,
        OrientationIndicator,
        OrientationArrow,
        CharacterCard,
        BurstLoadsBar;

    [SerializeField]
    public AudioClip spawnSfx;

    [SerializeField]
    Sound3DManager sound3DManager;

    const float SPAWN_SFX_VOLUME = 0.01f;

    public void ToggleSpawnFeedback(bool isActiveSound, string id)
    {
        spawnFeedback.SetActive(isActiveSound);
        if (isActiveSound)
        {
            MMSoundManagerSoundPlayEvent.Trigger(
                spawnSfx,
                MMSoundManager.MMSoundManagerTracks.Sfx,
                Utils.GetPlayer(GameServerConnectionManager.Instance.playerId).transform.position,
                false,
                SPAWN_SFX_VOLUME
            );
        }
    }
}
