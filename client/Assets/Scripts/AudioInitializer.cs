using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using System.Threading.Tasks;
using UnityEngine;

public class AudioInitializer : MonoBehaviour
{
    [SerializeField]
    private MMF_Player backgroundMusic;

    private async void Start()
    {
        backgroundMusic.PlayFeedbacks();
        await Task.Delay(1);
        MMSoundManager.Instance.MuteTrack(MMSoundManager.MMSoundManagerTracks.Master);
    }
}
