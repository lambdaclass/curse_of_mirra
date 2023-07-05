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

    [SerializeField]
    private MMSoundManager soundManager;

    private Image muteButtonImage;

    void Start()
    {
        muteButtonImage = GetComponent<Image>();
    }

    public void Toggle()
    {
        if (soundManager.IsMuted(MMSoundManager.MMSoundManagerTracks.Master))
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
