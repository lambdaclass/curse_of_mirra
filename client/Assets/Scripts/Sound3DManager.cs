using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class Sound3DManager : MonoBehaviour
{
    private MMF_Player mmf_player;

    void Start()
    {
        mmf_player = GetComponent<MMF_Player>();
    }

    public void SetSfxSound(AudioClip sfx)
    {
        mmf_player.GetFeedbackOfType<MMF_MMSoundManagerSound>().Sfx = sfx;
    }

    public void PlaySfxSound()
    {
        AudioClip sfx = mmf_player.GetFeedbackOfType<MMF_MMSoundManagerSound>().Sfx;
        if (sfx)
        {
            mmf_player.PlayFeedbacks();
        }
    }
}
