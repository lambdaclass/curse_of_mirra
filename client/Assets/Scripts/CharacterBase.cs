using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.VFX;

public class CharacterBase : MonoBehaviour
{
    [SerializeField]
    public GameObject PlayerName;

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
    public GameObject HealthBar;

    IEnumerator activateSpawnFeedback()
    {
        float lifeTime = spawnFeedback.GetComponent<VisualEffect>().GetFloat("LifeTime");
        spawnFeedback.SetActive(true);
        MMSoundManagerSoundPlayEvent.Trigger(
            spawnSfx,
            MMSoundManager.MMSoundManagerTracks.Sfx,
            Utils.GetPlayer(SocketConnectionManager.Instance.playerId).transform.position
        );
        yield return new WaitForSeconds(lifeTime);
        spawnFeedback.SetActive(false);
    }

    void Start()
    {
        StartCoroutine(activateSpawnFeedback());
    }
}
