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
        OrientationArrow;

    [SerializeField]
    public AudioClip spawnSfx;

    [SerializeField]
    Sound3DManager sound3DManager;

    public IEnumerator activateSpawnFeedback(bool isCurrentPlayer)
    {
        float lifeTime = spawnFeedback.GetComponent<VisualEffect>().GetFloat("LifeTime");
        spawnFeedback.SetActive(true);
        if (isCurrentPlayer)
        {
            MMSoundManagerSoundPlayEvent.Trigger(
                spawnSfx,
                MMSoundManager.MMSoundManagerTracks.Sfx,
                Utils.GetPlayer(SocketConnectionManager.Instance.playerId).transform.position
            );
        }
        else
        {
            sound3DManager.SetSfxSound(spawnSfx);
            sound3DManager.PlaySfxSound();
        }
        yield return new WaitForSeconds(lifeTime);
        spawnFeedback.SetActive(false);
    }
}
