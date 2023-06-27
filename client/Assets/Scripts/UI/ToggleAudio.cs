using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;

public class ToggleAudio : MonoBehaviour
{
    [SerializeField]
    public Sprite mutedSprite;

    [SerializeField]
    public Sprite unmutedSprite;

    private Image muteButtonImage;

    [SerializeField]
    private MMSoundManager soundManager;
    private bool isMuted;

    [SerializeField]
    private MMF_Player backgroundMusic;

    void Start()
    {
        muteButtonImage = GetComponent<Image>();
        isMuted = true;
        backgroundMusic.PlayFeedbacks();
        soundManager.SetVolumeSfx(0.5f);
        SilenceSound();
    }

    public void Toggle()
    {
        isMuted = !isMuted;

        if (isMuted)
        {
            SilenceSound();
            muteButtonImage.overrideSprite = mutedSprite;
        }
        else
        {
            PlaySound();
            muteButtonImage.overrideSprite = unmutedSprite;
        }
    }

    private void SilenceSound()
    {
        soundManager.PauseTrack(MMSoundManager.MMSoundManagerTracks.Music);
        soundManager.MuteMaster();
    }

    private void PlaySound()
    {
        soundManager.UnmuteMaster();
        soundManager.PlayTrack(MMSoundManager.MMSoundManagerTracks.Music);
    }
}
