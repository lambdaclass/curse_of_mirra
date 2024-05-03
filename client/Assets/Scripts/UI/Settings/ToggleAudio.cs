using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleAudio : MonoBehaviour
{
    [SerializeField]
    private Slider volumeSlider;

    private float unmutedVolume;

    private MMSoundManager soundManager;

    private Image muteButtonImage;

    private const float SFX_VOLUME = 10f;
    private const float MASTER_VOLUME = 5f;
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
        soundManager = MMSoundManager.Instance;
        soundManager.SetTrackVolume(MMSoundManager.MMSoundManagerTracks.Master, MASTER_VOLUME);
        SetUnmutedVolume(channel);
        ToggleUIState(!IsMuted(channel));
    }

    void Update()
    {
        if (volumeSlider)
        {
            bool unmuted = volumeSlider.value > MUTED_VOLUME;
            ToggleUIState(unmuted);
        }
    }

    private void ControlChannelTrack(MMSoundManager.MMSoundManagerTracks track, float baseVolume)
    {
        if (!IsMuted(channel))
        {
            float currentTrackVolume = soundManager.GetTrackVolume(track, false);
            float trackVolume = currentTrackVolume != baseVolume ? currentTrackVolume : baseVolume;
            unmutedVolume = volumeSlider ? volumeSlider.value : trackVolume;
        }
        else
        {
            unmutedVolume = volumeSlider ? volumeSlider.value : baseVolume;
        }
    }

    public void SetUnmutedVolume(MMSoundManager.MMSoundManagerTracks channel)
    {
        switch (channel)
        {
            case MMSoundManager.MMSoundManagerTracks.Music:
                ControlChannelTrack(channel, MUSIC_VOLUME);
                break;
            case MMSoundManager.MMSoundManagerTracks.Sfx:
                ControlChannelTrack(channel, SFX_VOLUME);
                break;
        }
    }

    public void Toggle()
    {
        bool isMuted = !IsMuted(channel);
        if (isMuted)
        {
            SilenceSound();
        }
        else
        {
            PlaySound();
        }
        ToggleUIState(isMuted);
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
            && soundManager.GetTrackVolume(track, false) <= MUTED_VOLUME;
    }

    private void ToggleUIState(bool isUnMuted)
    {
        muteButtonImage.enabled = isUnMuted;
        textSoundState.text = isUnMuted ? onState : offState;
        int alpha = isUnMuted ? 255 : 100;
        textSoundState.color = new Color32(255, 255, 255, (byte)alpha);
    }
}
