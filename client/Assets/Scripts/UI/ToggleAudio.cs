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

    [SerializeField]
    private MMSoundManager.MMSoundManagerTracks channel;

    void Start()
    {
        muteButtonImage = GetComponentInChildren<Image>();
        soundManager = MMSoundManager.Instance;
        unmutedVolume = volumeSlider ? volumeSlider.value : 1f;
        soundManager.SetVolumeSfx(SFX_VOLUME);
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

    public void ToggleMusicChannel()
    {
        if (IsMuted(MMSoundManager.MMSoundManagerTracks.Music))
        {
            PlaySound();
            muteButtonImage.overrideSprite = unmutedSprite;
        }
        else
        {
            SilenceSound();
            muteButtonImage.overrideSprite = unmutedSprite;
        }
    }

    private void SilenceSound()
    {
        unmutedVolume = volumeSlider.value;
        switch (channel)
        {
            case MMSoundManager.MMSoundManagerTracks.Music:
                soundManager.UnmuteMusic();
                soundManager.PauseTrack(channel);
                soundManager.MuteMusic();
                break;
            case MMSoundManager.MMSoundManagerTracks.Sfx:
                soundManager.UnmuteSfx();
                soundManager.PauseTrack(channel);
                soundManager.MuteSfx();
                break;
        }
    }

    private void PlaySound()
    {
        // SetVolume(unmutedVolume);
        switch (channel)
        {
            case MMSoundManager.MMSoundManagerTracks.Music:
                soundManager.UnmuteMusic();
                soundManager.PlayTrack(channel);
                break;
            case MMSoundManager.MMSoundManagerTracks.Sfx:
                soundManager.UnmuteSfx();
                soundManager.PlayTrack(channel);
                break;
        }
        SetVolume(unmutedVolume);
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
        return !soundManager.IsMuted(track) || soundManager.GetTrackVolume(track, false) <= 0.0001f;
    }
}
