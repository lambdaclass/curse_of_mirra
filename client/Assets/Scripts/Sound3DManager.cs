using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class Sound3DManager : MonoBehaviour
{
    public void SetSfxSound(AudioClip sfx)
    {
        GetComponent<MMF_Player>().GetFeedbackOfType<MMF_MMSoundManagerSound>().Sfx = sfx;
    }

    public void PlaySfxSound()
    {
        GetComponent<MMF_Player>().PlayFeedbacks();
    }
}
