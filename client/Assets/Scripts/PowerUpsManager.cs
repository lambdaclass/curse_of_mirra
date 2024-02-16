using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerUpsManager : MonoBehaviour
{
    [SerializeField] GameObject powerUpItem;
    private Dictionary<ulong, GameObject> availablePowerUps = new Dictionary<ulong, GameObject>();

    public void UpdatePowerUps(){
        GameServerConnectionManager.Instance.gamePowerUps.ForEach(powerupEntity => {
            PowerUp powerUp = powerupEntity.PowerUp;

            if(powerUp.Status == PowerUpstatus.Available && !availablePowerUps.Keys.Contains(powerupEntity.Id)){
                Vector3 pos = Utils.transformBackendOldPositionToFrontendPosition(powerupEntity.Position);
                GameObject powerupGameObject = Instantiate(powerUpItem, pos, Quaternion.identity);
                powerupGameObject.transform.localScale.Set(0.7f,0.7f,0.7f);
                availablePowerUps.Add(powerupEntity.Id, powerupGameObject);
            } 

            if(powerUp.Status == PowerUpstatus.Taken && availablePowerUps.Keys.Contains(powerupEntity.Id)){
                Destroy(availablePowerUps[powerupEntity.Id]);
                availablePowerUps.Remove(powerupEntity.Id);
            }
        });
    }

}
