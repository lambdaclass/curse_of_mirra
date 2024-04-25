using System;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI uiValue;
    private MMSoundManager soundManager;
    private Slider volumeSlider;

    [SerializeField]
    private MMSoundManager.MMSoundManagerTracks channelToUse;

    //The engines defines this value as 0 (muted)
    private const float MUTED_VOLUME = 0.0001f;

    void Awake()
    {
        soundManager = MMSoundManager.Instance;
    }

    void Start()
    {
        volumeSlider = GetComponent<Slider>();
        uiValue.text = UIVolumeValue(volumeSlider.value);
    }

    string UIVolumeValue(float value)
    {
        if (value <= MUTED_VOLUME)
        {
            return "0";
        }
        else if (value > MUTED_VOLUME && value < 0.01)
        {
            return "1";
        }
        else
        {
            return Mathf.FloorToInt(value * 100).ToString();
        }
    }

    private void MuteChannel()
    {
        switch (channelToUse)
        {
            case MMSoundManager.MMSoundManagerTracks.Music:
                MMSoundManager.Instance.MuteMusic();
                break;
            case MMSoundManager.MMSoundManagerTracks.Sfx:
                MMSoundManager.Instance.MuteSfx();
                break;
        }
    }

    public void ChangeMusicVolume()
    {
        if (IsMuted(channelToUse))
        {
            MuteChannel();
        }
        MMSoundManagerTrackEvent.Trigger(
            MMSoundManagerTrackEventTypes.SetVolumeTrack,
            channelToUse,
            volumeSlider.value
        );
        uiValue.text = UIVolumeValue(volumeSlider.value);
    }

    private void Update()
    {
        volumeSlider.value = soundManager.GetTrackVolume(channelToUse, false);
    }

    private bool IsMuted(MMSoundManager.MMSoundManagerTracks track)
    {
        return !soundManager.IsMuted(track) || soundManager.GetTrackVolume(track, false) <= 0.0001f;
    }
}
