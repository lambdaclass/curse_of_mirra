using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    private MMSoundManager soundManager;
    private Slider volumeSlider;

    void Start()
    {
        volumeSlider = GetComponent<Slider>();
        soundManager = MMSoundManager.Instance;

        volumeSlider.value = soundManager.GetTrackVolume(
            MMSoundManager.MMSoundManagerTracks.Music,
            false
        );
    }

    public void ChangeMusicVolume()
    {
        soundManager.SetVolumeMusic(volumeSlider.value);
    }

    private void Update()
    {
        volumeSlider.value = soundManager.GetTrackVolume(
            MMSoundManager.MMSoundManagerTracks.Music,
            false
        );
    }
}
