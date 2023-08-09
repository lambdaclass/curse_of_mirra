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

    void Start()
    {
        volumeSlider = GetComponent<Slider>();
        soundManager = MMSoundManager.Instance;

        volumeSlider.value = soundManager.GetTrackVolume(
            MMSoundManager.MMSoundManagerTracks.Master,
            false
        );
        uiValue.text = uiValue.text = Mathf.FloorToInt(volumeSlider.value * 100).ToString();
    }

    public void ChangeMusicVolume()
    {
        soundManager.SetVolumeMaster(volumeSlider.value);
        uiValue.text = Mathf.FloorToInt(volumeSlider.value * 100).ToString();
    }

    private void Update()
    {
        volumeSlider.value = soundManager.GetTrackVolume(
            MMSoundManager.MMSoundManagerTracks.Master,
            false
        );
    }
}
