using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;

public class ToggleAudio : MonoBehaviour
{
    public Image muteButtonImage;
    AudioSource[] sources;
    private bool isMuted;

    void Start()
    {
        sources = GameObject.FindObjectsOfType<AudioSource>();
        isMuted = false;
    }

    public void Toggle()
    {
        Debug.Log("Toggle");
        isMuted = !isMuted;

        foreach (AudioSource audioSource in sources)
        {
            if (isMuted)
            {
                audioSource.Pause();
            }
            else
            {
                audioSource.UnPause();
            }
        }
        // UpdateButtonImage();
    }

    void UpdateButtonImage()
    {
        if (isMuted)
        {
            muteButtonImage.sprite = Resources.Load<Sprite>(
                "Assets/ThirdParty/TopDownEngine/ThirdParty/MoreMountains/MMInterface/Styles/Mini/Sprites/Icons/MMiconSoundOff"
            );
        }
        else
        {
            muteButtonImage.sprite = Resources.Load<Sprite>(
                "Assets/ThirdParty/TopDownEngine/ThirdParty/MoreMountains/MMInterface/Styles/Mini/Sprites/Icons/MMIconSoundOn"
            );
        }
    }
}
