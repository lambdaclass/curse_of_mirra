using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StaminaManager : MonoBehaviour
{
    [SerializeField] private List<Image> staminaFillImage;
    private Player player;
    private Vector3 initialScale;
    private Vector3 initialparentScale;

    void Start(){
          player = Utils.GetGamePlayer(GameServerConnectionManager.Instance.playerId).Player;
          initialScale = staminaFillImage[0].transform.localScale;
          initialparentScale = staminaFillImage[0].transform.parent.localScale;
    }
   
    void Update(){
         player = Utils.GetGamePlayer(GameServerConnectionManager.Instance.playerId).Player;
         ulong availableStamina = player.AvailableStamina;
        staminaFillImage.ForEach(staminaCharge => {
            int index = staminaFillImage.IndexOf(staminaCharge);
            if(availableStamina == 0){
               StaminaAnimation(0, staminaCharge, availableStamina);
            } else {
               StaminaAnimation(index, staminaCharge, availableStamina);
            }
        });
    }

    private void StaminaAnimation(int index, Image element, ulong availableStamina){
        Vector3 scale = index <(int)availableStamina ? initialScale : Vector3.zero;
        Vector3 parentScale = index < (int)availableStamina ? initialparentScale + new Vector3(0.1f, 0.1f, 0.1f) : initialparentScale;
        float interval = scale == Vector3.zero ? 0.09f : 0.09f;
        element.transform.DOScale(scale,interval);
    }
    
}
