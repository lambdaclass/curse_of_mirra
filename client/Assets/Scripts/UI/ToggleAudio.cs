using MoreMountains.Tools;
using UnityEngine;

public class ToggleAudio : MonoBehaviour
{
    AudioSource[] sources;
    UnityEngine.UI.Button muteButton;

    void Start()
    {
        // Get every single audio sources in the scene.
        sources =
            GameObject.FindObjectsByType(typeof(AudioSource), FindObjectsSortMode.None)
            as AudioSource[];
        muteButton = GameObject.Find("MuteButton").GetComponent<UnityEngine.UI.Button>();
    }

    public void Toggle()
    {
        foreach (AudioSource audioSource in sources)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
                // Change button's image
                Debug.Log("Mute");
                muteButton.image.sprite = Resources.Load<Sprite>(
                    "Assets/ThirdParty/TopDownEngine/ThirdParty/MoreMountains/MMInterface/Styles/Mini/Sprites/Icons/MMiconSoundOff.png"
                );
            }
            else
            {
                audioSource.UnPause();
                // Change button's image
                Debug.Log("Unmute");
                muteButton.image.sprite = Resources.Load<Sprite>(
                    "Assets/ThirdParty/TopDownEngine/ThirdParty/MoreMountains/MMInterface/Styles/Mini/Sprites/Icons/MMIconSoundOn.png"
                );
            }
        }
    }
}
