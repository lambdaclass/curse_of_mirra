using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaManager : MonoBehaviour
{
    [SerializeField] private List<Image> staminaFillImage;

    private ulong maxStamina;    
    private Player player;
    private ulong currentStamina = 2;

    void Start(){
          player = Utils.GetGamePlayer(GameServerConnectionManager.Instance.playerId).Player;
          maxStamina = player.MaxStamina;
    }
   
    void Update(){
         player = Utils.GetGamePlayer(GameServerConnectionManager.Instance.playerId).Player;
         ulong availableStamina = player.AvailableStamina;

        staminaFillImage.ForEach(el => {
            int index = staminaFillImage.IndexOf(el);
            if(availableStamina == 0){
                // el.color = Utils.burstLoadsBarCharging;
                el.gameObject.SetActive(false);
            } else {
                // el.color = Utils.healthBarCyan;
              el.gameObject.SetActive(index < (int)availableStamina);
            }
        });
    }
}
