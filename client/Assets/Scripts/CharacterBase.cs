using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.VFX;

public class CharacterBase : MonoBehaviour
{
    [SerializeField]
    public GameObject Hitbox;

    [SerializeField]
    public GameObject FeedbackContainer;

    [SerializeField]
    public GameObject AimDirection;

    [SerializeField]
    public GameObject SkillRange;

    [SerializeField]
    public GameObject spawnFeedback;

    [SerializeField]
    public AudioClip spawnSfx;

    [SerializeField]
    Sound3DManager sound3DManager;

    public void ToggleSpawnFeedback(bool value, string id)
    {
        spawnFeedback.SetActive(value);
        if (value)
        {
            MMSoundManagerSoundPlayEvent.Trigger(
                spawnSfx,
                MMSoundManager.MMSoundManagerTracks.Sfx,
                Utils.GetPlayer(SocketConnectionManager.Instance.playerId).transform.position,
                false,
                0.001f
            );
        }
    }
}
