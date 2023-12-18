using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class Sound3DManager : MonoBehaviour
{
    private MMF_Player mmf_player;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private bool USING_MMF_PLAYER;

    void Start()
    {
        mmf_player = GetComponent<MMF_Player>();
    }

    public void SetSfxSound(AudioClip sfx)
    {
        mmf_player.GetFeedbackOfType<MMF_MMSoundManagerSound>().Sfx = sfx;
        audioSource.clip = sfx;
    }

    public void PlaySfxSound()
    {
        AudioClip sfx = mmf_player.GetFeedbackOfType<MMF_MMSoundManagerSound>().Sfx;
        if (sfx)
        {
            if (USING_MMF_PLAYER)
            {
                mmf_player.PlayFeedbacks();
            }
            else
            {
                audioSource.Play();
            }
        }
    }
}
