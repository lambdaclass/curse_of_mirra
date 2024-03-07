using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StaminaManager : MonoBehaviour
{
    [SerializeField]
    private List<Image> staminaContainerImages,
        staminaFillImage,
        glowEffects;

    [SerializeField]
    CustomCharacter character;
    private Player player;
    private Vector3 initialScale;
    public bool playingFeedback = false;
    ulong clientPlayer;

    void Start()
    {
        clientPlayer = GameServerConnectionManager.Instance.playerId;
        player = Utils.GetGamePlayer(clientPlayer).Player;
        if (UInt64.Parse(character.PlayerID) == clientPlayer)
        {
            foreach (Image stamina in staminaContainerImages)
            {
                stamina.gameObject.SetActive(true);
            }
            foreach (Image glowImage in glowEffects)
            {
                glowImage.color = new Color32(255, 0, 0, 255);
            }
        }

        initialScale = staminaFillImage[0].transform.localScale;
    }

    void Update()
    {
        player = Utils.GetGamePlayer(clientPlayer).Player;
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
        Sequence ShakeSequence = DOTween.Sequence();
        ShakeSequence.Append(
            transform.DOShakePosition(
                .5f,
                new Vector3(0, .1f, 0),
                10,
                0,
                false,
                true,
                ShakeRandomnessMode.Harmonic
            )
        );
        ShakeSequence.OnStart(() =>
        {
            playingFeedback = true;
            foreach (Image staminaImage in staminaContainerImages)
            {
                Sequence emptyFeedbackSequence = DOTween.Sequence();
                emptyFeedbackSequence
                    .Append(staminaImage.DOColor(Color.red, .4f))
                    .Append(staminaImage.DOColor(Color.white, .4f));
            }
            foreach (Image glowImage in glowEffects)
            {
                Sequence emptyFeedbackSequence = DOTween.Sequence();
                emptyFeedbackSequence
                    .Insert(0, glowImage.GetComponent<CanvasGroup>().DOFade(1, 0.1f))
                    .AppendInterval(.2f)
                    .Append(glowImage.GetComponent<CanvasGroup>().DOFade(0, 0.1f));
            }
        });
        ShakeSequence.OnComplete(() =>
        {
            playingFeedback = false;
        });
    }
}
