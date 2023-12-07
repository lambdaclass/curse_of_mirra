using UnityEngine;

public class Inventory : MonoBehaviour {
    private Player player;
    private bool active;
    private GameLoot activeLoot;
    [SerializeField]
    public GameObject inventoryButton;

    [SerializeField]
    

    private void Awake() {
        player = Utils.GetGamePlayer(LobbyConnection.Instance.playerId);
        
    }

    private void Update() {
        if (!active && player.Inventory.Count > 0) {
            active = true;
            activeLoot = player.Inventory[0]; // GameLoot
        }
        
        // if (activeLoot != null) {

        // }
    }    
}
