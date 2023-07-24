using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [SerializeField]
    private MMSoundManager soundManager;
    private Slider volumeSlider;

    void Awake()
    {
        volumeSlider = GetComponent<Slider>();
        volumeSlider.value = soundManager.GetTrackVolume(
            MMSoundManager.MMSoundManagerTracks.Music,
            false
        );
    }

    public void ChangeMusicVolume()
    {
        soundManager.SetVolumeMusic(volumeSlider.value);
    }
}
