using MoreMountains.TopDownEngine;
using UnityEngine;

public class SpawnLoot : MonoBehaviour
{
    [SerializeField]
    private GameObject lootHealth;

    public void Init()
    {
        Debug.Log("SpawnLoot");
        var lootInstance = Instantiate(
            lootHealth,
            // new Vector3(Random.Range(-30f, 30f), 1f, Random.Range(-30f, 30f)),
            new Vector3(0, 1f, 0),
            Quaternion.identity
        );
    }

    public void PickUp()
    {
        Debug.Log("PickUp");
    }
}
