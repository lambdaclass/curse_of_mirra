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
    private MMSoundManager soundManager;
    private bool isMuted;
    private MMF_Player backgroundMusic;

    void Awake()
    {
        soundManager = FindObjectOfType<MMSoundManager>();
        backgroundMusic = GameObject.Find("BackgroundMusic").GetComponent<MMF_Player>();
        muteButtonImage = GetComponent<Image>();
        isMuted = true;
    }

    public void Toggle()
    {
        isMuted = !isMuted;

        if (isMuted)
        {
            soundManager.StopAllSounds();
            muteButtonImage.overrideSprite = mutedSprite;
        }
        else
        {
            backgroundMusic.PlayFeedbacks();
            soundManager.UnmuteMaster();
            muteButtonImage.overrideSprite = unmutedSprite;
        }
    }
}
