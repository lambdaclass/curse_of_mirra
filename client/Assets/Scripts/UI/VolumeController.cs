using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    private MMSoundManager soundManager;
    private Slider volumeSlider;
    private float unmutedVolume;

    void Start()
    {
        volumeSlider = GetComponent<Slider>();
        soundManager = MMSoundManager.Instance;

        volumeSlider.value = soundManager.GetTrackVolume(
            MMSoundManager.MMSoundManagerTracks.Master,
            false
        );

        unmutedVolume = volumeSlider.value;
    }

    public void ChangeMusicVolume()
    {
        if (IsMuted(MMSoundManager.MMSoundManagerTracks.Master))
        {
            MMSoundManager.Instance.UnmuteMaster();
        }

        MMSoundManagerTrackEvent.Trigger(
            MMSoundManagerTrackEventTypes.SetVolumeTrack,
            MMSoundManager.MMSoundManagerTracks.Master,
            volumeSlider.value
        );
    }

    private void Update()
    {
        volumeSlider.value = soundManager.GetTrackVolume(
            MMSoundManager.MMSoundManagerTracks.Master,
            false
        );
    }

    private bool IsMuted(MMSoundManager.MMSoundManagerTracks track)
    {
        return !soundManager.IsMuted(track) || soundManager.GetTrackVolume(track, false) <= 0.0001f;
    }
}
