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

    void Start()
    {
        volumeSlider = GetComponent<Slider>();
        soundManager = MMSoundManager.Instance;

        volumeSlider.value = soundManager.GetTrackVolume(
            MMSoundManager.MMSoundManagerTracks.Master,
            false
        );

        unmutedVolume = volumeSlider.value;
        uiValue.text = uiValue.text = Mathf.FloorToInt(volumeSlider.value * 100).ToString();
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
        uiValue.text = Mathf.FloorToInt(volumeSlider.value * 100).ToString();
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
