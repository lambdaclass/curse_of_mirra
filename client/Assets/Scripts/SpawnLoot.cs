using UnityEngine;

public class SpawnLoot : MonoBehaviour
{
    [SerializeField]
    private GameObject lootHealth;
    private GameObject lootInstance;

    // This method simulates the backend spawning a loot in the center of the map.
    // TODO: public void Init(LootPackage lootPackage, Vector3 position)
    public void Init()
    {
        Debug.Log("SpawnLoot");
        lootInstance = Instantiate(lootHealth, new Vector3(0, 1f, 0), Quaternion.identity);
        //lootInstance = Instantiate(lootHealth, position, Quaternion.identity);
    }

    public void PickUp()
    {
        Debug.Log("PickUp");
        Destroy(lootInstance);
    }
}
