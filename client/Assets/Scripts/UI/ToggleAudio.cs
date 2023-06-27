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
        soundManager.MuteMaster();
    }

    public void Toggle()
    {
        isMuted = !isMuted;

        if (isMuted)
        {
            soundManager.MuteMaster();
            muteButtonImage.overrideSprite = mutedSprite;
        }
        else
        {
            soundManager.StopAllSounds();
            soundManager.UnmuteMaster();
            backgroundMusic.PlayFeedbacks();
            muteButtonImage.overrideSprite = unmutedSprite;
        }
    }
}
