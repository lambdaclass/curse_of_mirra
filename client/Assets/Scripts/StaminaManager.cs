using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StaminaManager : MonoBehaviour
{
    [SerializeField] private List<Image> staminaFillImage;

    private ulong maxStamina;    
    private Player player;
    private ulong currentStamina = 2;

    private Vector3 initialScale;

    void Start(){
          player = Utils.GetGamePlayer(GameServerConnectionManager.Instance.playerId).Player;
          maxStamina = player.MaxStamina;
          initialScale = staminaFillImage[0].transform.localScale;
    }
   
    void Update(){
         player = Utils.GetGamePlayer(GameServerConnectionManager.Instance.playerId).Player;
         ulong availableStamina = player.AvailableStamina;

        staminaFillImage.ForEach(el => {
            int index = staminaFillImage.IndexOf(el);
            if(availableStamina == 0){
                el.transform.localScale = Vector3.zero;
            } else {
                Vector3 scale = index < (int)availableStamina ? initialScale : Vector3.zero;
                float interval = scale == Vector3.zero ? 0.1f : 0.3f;
                el.transform.DOScale(scale,interval);
            }
        });
    }
}
