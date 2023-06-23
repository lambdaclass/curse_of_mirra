using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    private MMSoundManager soundManager;
    private Slider masterVolumeSlider;

    // Start is called before the first frame update
    void Awake()
    {
        soundManager = FindObjectOfType<MMSoundManager>();
        masterVolumeSlider = GameObject.Find("MasterVolumeSlider").GetComponent<Slider>();
    }

    // Update is called once per frame
    public void ChangeVolume()
    {
        soundManager.SetVolumeMaster(masterVolumeSlider.value);
    }
}
