using MoreMountains.Feedbacks;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class SpawnLoot : MonoBehaviour
{
    GameObject lootInstance;
    // public MMFeedbacks PickedMMFeedbacks;

    public void Initialize()
    {
        print(lootInstance);
        if (lootInstance == null)
        {
            Init("LootHealth", new Vector3(Random.Range(-30f, 30f), 1f, Random.Range(-30f, 30f)));
        }
        else
        {
            PickUp();
        }
    }
    // Start is called before the first frame update
    void Init(string prefab, Vector3 position)
    {
        lootInstance = Instantiate(Resources.Load(prefab, typeof(GameObject))) as GameObject;
        lootInstance.transform.position = position;
        // lootInstance.GetComponent<PickableItem>().PickedMMFeedbacks.PlayFeedbacks();
        // PickedMMFeedbacks?.Initialization(lootInstance);
    }
    // Update is called once per frame
    public void PickUp()
    {
        print(lootInstance);
        lootInstance.GetComponent<PickableItem>().PickedMMFeedbacks.PlayFeedbacks();
        Destroy(lootInstance, 0.1f);
    }
}
