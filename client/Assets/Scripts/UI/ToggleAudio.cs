using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;

public class ToggleAudio : MonoBehaviour
{
    [SerializeField]
    public Sprite mutedSprite;

    [SerializeField]
    public Sprite unmutedSprite;

    [SerializeField]
    private Slider volumeSlider;

    private float unmutedVolume;

    private MMSoundManager soundManager;

    private Image muteButtonImage;

    private ulong SFX_VOLUME = 3;

    void Start()
    {
        muteButtonImage = GetComponentInChildren<Image>();
        soundManager = MMSoundManager.Instance;
        unmutedVolume = volumeSlider ? volumeSlider.value : 1f;
        soundManager.SetVolumeSfx(SFX_VOLUME);
    }

    void Update()
    {
        if (!IsMuted(MMSoundManager.MMSoundManagerTracks.Master))
        {
            muteButtonImage.overrideSprite = unmutedSprite;
        }
        else
        {
            muteButtonImage.overrideSprite = mutedSprite;
        }
    }

    public void Toggle()
    {
        if (IsMuted(MMSoundManager.MMSoundManagerTracks.Master))
        {
            PlaySound();
            muteButtonImage.overrideSprite = unmutedSprite;
        }
        else
        {
            SilenceSound();
            muteButtonImage.overrideSprite = mutedSprite;
        }
    }

    private void SilenceSound()
    {
        unmutedVolume = volumeSlider ? volumeSlider.value : 1f;
        soundManager.PauseTrack(MMSoundManager.MMSoundManagerTracks.Music);
        soundManager.MuteMaster();
    }

    private void PlaySound()
    {
        soundManager.UnmuteMaster();
        SetVolume(unmutedVolume);
        soundManager.PlayTrack(MMSoundManager.MMSoundManagerTracks.Music);
    }

    private void SetVolume(float newVolume)
    {
        if (volumeSlider != null)
        {
            soundManager.SetVolumeMaster(newVolume);
        }
    }

    private bool IsMuted(MMSoundManager.MMSoundManagerTracks track)
    {
        // This may seem wrong, but it's not. The IsMuted() method does exactly the opposite of what its name suggests.
        return !soundManager.IsMuted(track) || soundManager.GetTrackVolume(track, false) <= 0.0001f;
    }
}
