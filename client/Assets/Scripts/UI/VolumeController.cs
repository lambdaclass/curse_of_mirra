using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    private MMSoundManager soundManager;
    private Slider musicVolumeSlider;
    private Slider sfxVolumeSlider;

    void Awake()
    {
        soundManager = FindObjectOfType<MMSoundManager>();
        musicVolumeSlider = GameObject.Find("MusicVolumeSlider").GetComponent<Slider>();
        sfxVolumeSlider = GameObject.Find("SfxVolumeSlider").GetComponent<Slider>();
    }

    public void ChangeMusicVolume()
    {
        soundManager.SetVolumeMusic(musicVolumeSlider.value);
    }

    public void ChangeSfxVolume()
    {
        soundManager.SetVolumeSfx(sfxVolumeSlider.value);
    }
}
