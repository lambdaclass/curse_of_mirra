using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;

public class ToggleAudio : MonoBehaviour
{
    [SerializeField]
    private Sprite mutedSprite;

    [SerializeField]
    private Sprite unmutedSprite;
    private Image muteButtonImage;
    AudioSource[] sources;
    private bool isMuted;

    void Awake()
    {
        sources = GameObject.FindObjectsOfType<AudioSource>();
        isMuted = false;
        muteButtonImage = GetComponentInChildren<Image>();
    }

    public void Toggle()
    {
        isMuted = !isMuted;

        foreach (AudioSource audioSource in sources)
        {
            if (isMuted)
            {
                audioSource.Pause();
                muteButtonImage.sprite = mutedSprite;
            }
            else
            {
                audioSource.UnPause();
                muteButtonImage.sprite = unmutedSprite;
            }
        }
    }
}
