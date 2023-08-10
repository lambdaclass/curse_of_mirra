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

    void Start()
    {
        muteButtonImage = GetComponent<Image>();
        soundManager = MMSoundManager.Instance;
        unmutedVolume = volumeSlider ? volumeSlider.value : 1f;
    }

    void Update()
    {
        // This may seem wrong, but it's not. The IsMuted() method does exactly the opposite of what its name suggests.
        if (!IsMuted(MMSoundManager.MMSoundManagerTracks.Master))
        {
            print("Update() IsMuted -> false");
            muteButtonImage.overrideSprite = unmutedSprite;
        }
        else
        {
            print("Update() IsMuted -> true");
            muteButtonImage.overrideSprite = mutedSprite;
        }
    }

    // public void Toggle()
    // {
    //     // This may seem wrong, but it's not. The IsMuted() method does exactly the opposite of what its name suggests.
    //     if (!IsMuted(MMSoundManager.MMSoundManagerTracks.Master))
    //     {
    //         print("Toggle() IsMuted -> false");
    //         SilenceSound();
    //         muteButtonImage.overrideSprite = mutedSprite;
    //     }
    //     else
    //     {
    //         print("Toggle() IsMuted -> true");
    //         PlaySound();
    //         muteButtonImage.overrideSprite = unmutedSprite;
    //     }
    // }

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

    // private void SilenceSound()
    // {
    //     unmutedVolume = volumeSlider ? volumeSlider.value : 1f;
    //     soundManager.PauseTrack(MMSoundManager.MMSoundManagerTracks.Music);
    //     soundManager.MuteMaster();
    //     //soundManager.SetVolumeMaster(0.0001f);
    // }

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
        //soundManager.SetVolumeMaster(1f);
        soundManager.PlayTrack(MMSoundManager.MMSoundManagerTracks.Music);
    }

    // private void SetVolume()
    // {
    //     if (volumeSlider != null)
    //     {
    //         print("Volume slider is null");
    //         if (IsMuted(MMSoundManager.MMSoundManagerTracks.Master))
    //         {
    //             soundManager.SetVolumeMaster(unmutedVolume);
    //         }
    //         else
    //         {
    //             soundManager.SetVolumeMaster(volumeSlider.value);
    //         }
    //     }
    //     else
    //     {
    //         soundManager.SetVolumeMaster(1f);
    //     }
    // }

    private void SetVolume(float newVolume)
    {
        if (volumeSlider != null)
        {
            soundManager.SetVolumeMaster(newVolume);
        }
    }

    private bool IsMuted(MMSoundManager.MMSoundManagerTracks track)
    {
        return !soundManager.IsMuted(track) || soundManager.GetTrackVolume(track, false) <= 0.0001f;
    }
}
