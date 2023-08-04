using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;

public class ToggleAudio : MonoBehaviour
{
    [SerializeField]
    public Sprite mutedSprite;

    [SerializeField]
    public Sprite unmutedSprite;
    private MMSoundManager soundManager;

    private Image muteButtonImage;

    void Start()
    {
        muteButtonImage = GetComponent<Image>();
        soundManager = MMSoundManager.Instance;
    }

    void Update()
    {
        // This may seem wrong, but it's not. The IsMuted() method does exactly the opposite of what its name suggests.
        if (!soundManager.IsMuted(MMSoundManager.MMSoundManagerTracks.Master))
        {
            muteButtonImage.overrideSprite = mutedSprite;
        }
        else
        {
            muteButtonImage.overrideSprite = unmutedSprite;
        }
    }

    public void Toggle()
    {
        // This may seem wrong, but it's not. The IsMuted() method does exactly the opposite of what its name suggests.
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
        soundManager.SetVolumeMaster(1);
        soundManager.PlayTrack(MMSoundManager.MMSoundManagerTracks.Music);
    }
}
