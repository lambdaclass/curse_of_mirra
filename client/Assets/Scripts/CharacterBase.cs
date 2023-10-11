using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

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

    public void activateSpawnFeedback()
    {
        spawnFeedback.SetActive(true);
        MMSoundManagerSoundPlayEvent.Trigger(
            spawnSfx,
            MMSoundManager.MMSoundManagerTracks.Sfx,
            Utils.GetPlayer(SocketConnectionManager.Instance.playerId).transform.position
        );
    }

    void Start()
    {
        activateSpawnFeedback();
    }
}
