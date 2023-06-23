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
    AudioSource[] sources;
    private bool isMuted = false;

    void Awake()
    {
        sources = GameObject.FindObjectsOfType<AudioSource>();
        soundManager = FindObjectOfType<MMSoundManager>();
        isMuted = false;
        muteButtonImage = GetComponent<Image>();
    }

    public void Toggle()
    {
        isMuted = !isMuted;

        foreach (AudioSource audioSource in sources)
        {
            if (isMuted)
            {
                audioSource.Pause();
                muteButtonImage.overrideSprite = mutedSprite;
            }
            else
            {
                audioSource.UnPause();
                muteButtonImage.overrideSprite = unmutedSprite;
            }
        }
    }
}
