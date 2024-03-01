using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StaminaManager : MonoBehaviour
{
    [SerializeField]
    private List<Image> staminaContainerImages,
        staminaFillImage;
    private Player player;
    private Vector3 initialScale;
    public bool playingFeedback = false;

    void Start()
    {
        player = Utils.GetGamePlayer(GameServerConnectionManager.Instance.playerId).Player;
        initialScale = staminaFillImage[0].transform.localScale;
    }

    void Update()
    {
        player = Utils.GetGamePlayer(GameServerConnectionManager.Instance.playerId).Player;
        ulong availableStamina = player.AvailableStamina;
        staminaFillImage.ForEach(staminaCharge =>
        {
            int index = staminaFillImage.IndexOf(staminaCharge);
            if (availableStamina == 0)
            {
                StaminaAnimation(0, staminaCharge, availableStamina);
            }
            else
            {
                StaminaAnimation(index, staminaCharge, availableStamina);
            }
        });
    }

    private void StaminaAnimation(int index, Image element, ulong availableStamina)
    {
        Vector3 scale = index < (int)availableStamina ? initialScale : Vector3.zero;
        float interval = scale == Vector3.zero ? 0.09f : 0.09f;
        element.transform.DOScale(scale, interval);
    }

    public void UnavailableStaminaFeedback()
    {
        playingFeedback = true;
        if (playingFeedback)
        {
            foreach (Image staminaImage in staminaContainerImages)
            {
                Sequence emptyFeedbackSequence = DOTween.Sequence();
                emptyFeedbackSequence
                    .Append(staminaImage.DOColor(Color.red, .1f))
                    .Append(staminaImage.DOColor(Color.white, .1f));
            }
            Sequence ShakeSequence = DOTween.Sequence();
            ShakeSequence.Append(
                transform.DOShakePosition(
                    .5f,
                    new Vector3(0, .5f, 0),
                    10,
                    0,
                    false,
                    true,
                    ShakeRandomnessMode.Harmonic
                )
            );
            ShakeSequence.OnComplete(() =>
            {
                playingFeedback = false;
            });
        }
    }
}
