using System.Threading.Tasks;
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

    async void Start()
    {
        muteButtonImage = GetComponent<Image>();
        soundManager = MMSoundManager.Instance;
        await Task.Delay(1);
        print(
            "Master is muted? "
                + MMSoundManager.Instance.IsMuted(MMSoundManager.MMSoundManagerTracks.Master)
        );
        if (soundManager.IsMuted(MMSoundManager.MMSoundManagerTracks.Master))
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
        print(
            "Master is muted? "
                + MMSoundManager.Instance.IsMuted(MMSoundManager.MMSoundManagerTracks.Master)
        );
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
