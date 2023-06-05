using MoreMountains.TopDownEngine;
using UnityEngine;

public class SpawnLoot : MonoBehaviour
{
    GameObject lootInstance;

    public void Initialize()
    {
        if (lootInstance == null)
        {
            Init("LootHealth", new Vector3(Random.Range(-30f, 30f), 1f, Random.Range(-30f, 30f)));
        }
        else
        {
            PickUp();
        }
    }
    void Init(string prefab, Vector3 position)
    {
        lootInstance = Instantiate(Resources.Load(prefab, typeof(GameObject))) as GameObject;
        lootInstance.transform.position = position;
    }
    public void PickUp()
    {
        lootInstance.GetComponent<PickableItem>().PickedMMFeedbacks.PlayFeedbacks();
        Destroy(lootInstance, 0.1f);
    }
}
