using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class UISoundManager : MonoBehaviour
{
    public MMSoundManager soundManager;
    public float sfxVolume = 2f;
    public float masterVolume = 0.7f;
    public float musicVolume = 0.5f;

    //The engines defines this value as 0 (muted)
    public float mutedVolume = 0.0001f;

    // Start is called before the first frame update
    void Start()
    {
        SetMusic();
    }

    void SetMusic()
    {
        soundManager = MMSoundManager.Instance;
        soundManager.SetTrackVolume(MMSoundManager.MMSoundManagerTracks.Master, masterVolume);
        soundManager.SetVolumeSfx(sfxVolume);
    }
}
