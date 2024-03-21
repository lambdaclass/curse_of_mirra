using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleAudio : MonoBehaviour
{
    [SerializeField]
    UISoundManager uiSoundManager;

    [SerializeField]
    public Sprite mutedSprite;

    [SerializeField]
    public Sprite unmutedSprite;

    [SerializeField]
    private Slider volumeSlider;

    private float unmutedVolume;

    private Image muteButtonImage;

    [SerializeField]
    private MMSoundManager.MMSoundManagerTracks channel;

    [SerializeField]
    TextMeshProUGUI textSoundState;
    string offState = "OFF";
    string onState = "ON";

    void Awake()
    {
        muteButtonImage = GetComponent<Image>();
        SetUnmutedVolume();
        if (IsMuted(channel))
        {
            UpdateMutedUIState();
        }
        else
        {
            UpdateUnmutedUIState();
        }
    }

    void Update()
    {
        if (
            volumeSlider
            && (IsMuted(channel) && unmutedVolume != volumeSlider.value)
            && volumeSlider.value > uiSoundManager.mutedVolume
        )
        {
            unmutedVolume = volumeSlider.value;
        }

        if (volumeSlider)
        {
            muteButtonImage.overrideSprite =
                volumeSlider.value == uiSoundManager.mutedVolume ? mutedSprite : unmutedSprite;
        }
    }

    public void SetUnmutedVolume()
    {
        if (!IsMuted(channel))
        {
            float currentMusicVolume = uiSoundManager
                .soundManager
                .GetTrackVolume(MMSoundManager.MMSoundManagerTracks.Music, false);
            float musicVolume =
                currentMusicVolume != uiSoundManager.musicVolume
                    ? currentMusicVolume
                    : uiSoundManager.musicVolume;
            unmutedVolume = volumeSlider ? volumeSlider.value : musicVolume;
        }
        else
        {
            unmutedVolume = volumeSlider ? volumeSlider.value : uiSoundManager.musicVolume;
        }
    }

    public void Toggle()
    {
        if (IsMuted(channel))
        {
            PlaySound();
            UpdateUnmutedUIState();
        }
        else
        {
            SilenceSound();
            UpdateMutedUIState();
        }
    }

    private void UpdateUnmutedUIState()
    {
        if (unmutedSprite != null)
        {
            muteButtonImage.enabled = true;
            muteButtonImage.overrideSprite = unmutedSprite;
        }
        else if (unmutedSprite == null)
        {
            muteButtonImage.enabled = false;
        }
        textSoundState.text = onState;
    }

    private void UpdateMutedUIState()
    {
        if (mutedSprite != null)
        {
            muteButtonImage.enabled = true;
            muteButtonImage.overrideSprite = mutedSprite;
        }
        else
        {
            muteButtonImage.enabled = false;
        }
        textSoundState.text = offState;
    }

    private void SilenceSound()
    {
        SetUnmutedVolume();
        switch (channel)
        {
            case MMSoundManager.MMSoundManagerTracks.Music:
                uiSoundManager.soundManager.MuteMusic();
                break;
            case MMSoundManager.MMSoundManagerTracks.Sfx:
                uiSoundManager.soundManager.MuteSfx();
                break;
        }
        uiSoundManager.soundManager.PauseTrack(channel);
    }

    private void PlaySound()
    {
        switch (channel)
        {
            case MMSoundManager.MMSoundManagerTracks.Music:
                uiSoundManager.soundManager.UnmuteMusic();
                break;
            case MMSoundManager.MMSoundManagerTracks.Sfx:
                uiSoundManager.soundManager.UnmuteSfx();
                break;
        }
        SetVolume(unmutedVolume);
        uiSoundManager.soundManager.PlayTrack(channel);
    }

    private void SetVolume(float newVolume)
    {
        switch (channel)
        {
            case MMSoundManager.MMSoundManagerTracks.Music:
                uiSoundManager.soundManager.SetVolumeMusic(newVolume);
                break;
            case MMSoundManager.MMSoundManagerTracks.Sfx:
                uiSoundManager.soundManager.SetVolumeSfx(newVolume);
                break;
        }
    }

    private bool IsMuted(MMSoundManager.MMSoundManagerTracks track)
    {
        // This may seem wrong, but it's not. The IsMuted() method does exactly the opposite of what its name suggests.
        return !uiSoundManager.soundManager.IsMuted(track)
            || uiSoundManager.soundManager.GetTrackVolume(track, false)
                <= uiSoundManager.mutedVolume;
    }
}
