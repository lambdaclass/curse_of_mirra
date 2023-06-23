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
    private bool isMuted = false;

    void Awake()
    {
        soundManager = FindObjectOfType<MMSoundManager>();
        isMuted = false;
        muteButtonImage = GetComponent<Image>();
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
            soundManager.UnmuteMaster();
            muteButtonImage.overrideSprite = unmutedSprite;
        }
    }
}
