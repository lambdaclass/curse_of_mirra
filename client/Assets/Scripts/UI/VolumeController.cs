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
    private float unmutedVolume;

    [SerializeField]
    private MMSoundManager.MMSoundManagerTracks channelToUse;

    void Awake()
    {
        soundManager = MMSoundManager.Instance;
        soundManager.SetTrackVolume(MMSoundManager.MMSoundManagerTracks.Master, 1);
    }

    void Start()
    {
        volumeSlider = GetComponent<Slider>();
        unmutedVolume = volumeSlider.value;
        uiValue.text = uiValue.text = Mathf.FloorToInt(volumeSlider.value * 100).ToString();
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
        uiValue.text = Mathf.FloorToInt(volumeSlider.value * 100).ToString();
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
