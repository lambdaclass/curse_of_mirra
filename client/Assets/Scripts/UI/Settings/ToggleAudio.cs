using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleAudio : MonoBehaviour
{
    [SerializeField]
    public Sprite unmutedSprite;

    [SerializeField]
    private Slider volumeSlider;

    private float unmutedVolume;

    private MMSoundManager soundManager;

    private Image muteButtonImage;

    private const float SFX_VOLUME = 2f;
    private const float MASTER_VOLUME = 0.7f;
    private const float MUSIC_VOLUME = 0.5f;

    //The engines defines this value as 0 (muted)
    private const float MUTED_VOLUME = 0.0001f;

    [SerializeField]
    private MMSoundManager.MMSoundManagerTracks channel;

    [SerializeField]
    TextMeshProUGUI textSoundState;
    string offState = "OFF";
    string onState = "ON";

    void Start()
    {
        muteButtonImage = GetComponent<Image>();
        muteButtonImage.sprite = unmutedSprite;
        soundManager = MMSoundManager.Instance;
        soundManager.SetTrackVolume(MMSoundManager.MMSoundManagerTracks.Master, MASTER_VOLUME);
        SetUnmutedVolume(channel);
        if (IsMuted(channel))
        {
            ToggleUIState(false);
        }
        else
        {
            ToggleUIState(true);
        }
    }

    void Update()
    {
        if (volumeSlider)
        {
            if (volumeSlider.value == MUTED_VOLUME)
            {
                ToggleUIState(false);
            }
            if (volumeSlider.value > MUTED_VOLUME)
            {
                ToggleUIState(true);
            }
        }
    }

    public void SetUnmutedVolume(MMSoundManager.MMSoundManagerTracks channel)
    {
        switch (channel)
        {
            case MMSoundManager.MMSoundManagerTracks.Music:
                if (!IsMuted(channel))
                {
                    float currentMusicVolume = soundManager.GetTrackVolume(
                        MMSoundManager.MMSoundManagerTracks.Music,
                        false
                    );
                    float musicVolume =
                        currentMusicVolume != MUSIC_VOLUME ? currentMusicVolume : MUSIC_VOLUME;
                    unmutedVolume = volumeSlider ? volumeSlider.value : musicVolume;
                }
                else
                {
                    unmutedVolume = volumeSlider ? volumeSlider.value : MUSIC_VOLUME;
                }
                break;
            case MMSoundManager.MMSoundManagerTracks.Sfx:
                if (!IsMuted(channel))
                {
                    float currentSFXVolume = soundManager.GetTrackVolume(
                        MMSoundManager.MMSoundManagerTracks.Sfx,
                        false
                    );
                    float sfxVolume =
                        currentSFXVolume != SFX_VOLUME ? currentSFXVolume : SFX_VOLUME;
                    unmutedVolume = volumeSlider ? volumeSlider.value : sfxVolume;
                }
                else
                {
                    unmutedVolume = volumeSlider ? volumeSlider.value : SFX_VOLUME;
                }
                break;
        }
    }

    public void Toggle()
    {
        if (IsMuted(channel))
        {
            PlaySound();
            ToggleUIState(true);
        }
        else
        {
            SilenceSound();
            ToggleUIState(false);
        }
    }

    private void SilenceSound()
    {
        SetUnmutedVolume(channel);
        switch (channel)
        {
            case MMSoundManager.MMSoundManagerTracks.Music:
                soundManager.MuteMusic();
                break;
            case MMSoundManager.MMSoundManagerTracks.Sfx:
                soundManager.MuteSfx();
                break;
        }
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

    private void ToggleUIState(bool isUnMuted)
    {
        muteButtonImage.enabled = isUnMuted;
        textSoundState.text = isUnMuted ? onState : offState;
        int alpha = isUnMuted ? 255 : 100;
        textSoundState.color = new Color32(255, 255, 255, (byte)alpha);
    }
}
