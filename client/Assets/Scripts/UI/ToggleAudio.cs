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

    //The engines defines this value as 0 (muted)
    private float MUTED_VOLUME = 0.0001f;

    [SerializeField]
    private MMSoundManager.MMSoundManagerTracks channel;

    void Start()
    {
        muteButtonImage = GetComponentInChildren<Image>();
        soundManager = MMSoundManager.Instance;
        soundManager.SetTrackVolume(MMSoundManager.MMSoundManagerTracks.Master, 1);
        unmutedVolume = volumeSlider ? volumeSlider.value : 1f;
        soundManager.SetVolumeSfx(SFX_VOLUME);
        muteButtonImage.overrideSprite = IsMuted(channel) ? mutedSprite : unmutedSprite;
    }

    void Update()
    {
        if (
            volumeSlider
            && (IsMuted(channel) && unmutedVolume != volumeSlider.value)
            && volumeSlider.value > MUTED_VOLUME
        )
        {
            unmutedVolume = volumeSlider.value;
        }

        if (volumeSlider)
        {
            muteButtonImage.overrideSprite =
                volumeSlider.value == MUTED_VOLUME ? mutedSprite : unmutedSprite;
        }
    }

    public void Toggle()
    {
        if (IsMuted(channel))
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
        switch (channel)
        {
            case MMSoundManager.MMSoundManagerTracks.Music:
                soundManager.MuteMusic();
                break;
            case MMSoundManager.MMSoundManagerTracks.Sfx:
                soundManager.MuteSfx();
                break;
        }
        soundManager.PauseTrack(channel);
    }

    private void PlaySound()
    {
        switch (channel)
        {
            case MMSoundManager.MMSoundManagerTracks.Music:
                soundManager.UnmuteMusic();
                break;
            case MMSoundManager.MMSoundManagerTracks.Sfx:
                soundManager.UnmuteSfx();
                break;
        }
        SetVolume(unmutedVolume);
        soundManager.PlayTrack(channel);
    }

    private void SetVolume(float newVolume)
    {
        switch (channel)
        {
            case MMSoundManager.MMSoundManagerTracks.Music:
                soundManager.SetVolumeMusic(newVolume);
                break;
            case MMSoundManager.MMSoundManagerTracks.Sfx:
                soundManager.SetVolumeSfx(newVolume);
                break;
        }
    }

    private bool IsMuted(MMSoundManager.MMSoundManagerTracks track)
    {
        // This may seem wrong, but it's not. The IsMuted() method does exactly the opposite of what its name suggests.
        return !soundManager.IsMuted(track)
            || soundManager.GetTrackVolume(track, false) <= MUTED_VOLUME;
    }
}
