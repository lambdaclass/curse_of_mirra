using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Loot : MonoBehaviour
{
    [SerializeField]
    GameObject healthLootObject;

    private Dictionary<ulong, GameObject> loots = new Dictionary<ulong, GameObject>();

    public void MaybeAddLoot(LootPackage loot)
    {
        if (!loots.ContainsKey(loot.Id))
        {
            var position = Utils.transformBackendPositionToFrontendPosition(loot.Position);
            GameObject lootObject = Instantiate(this.healthLootObject, position, Quaternion.identity);
            loots.Add(loot.Id, lootObject);
        }
    }

    public void RemoveLoots(List<LootPackage> updatedLoots)
    {
        var idsToRemove = this.loots.Keys.Except(updatedLoots.Select(loot => loot.Id)).ToList();
        idsToRemove.ForEach(RemoveLoot);
    }

    private GameObject getLootObject(LootType lootType)
    {
        switch (lootType)
        {
            case LootType.LootHealth: return this.healthLootObject;
            default: throw new ArgumentException("Type for loot ");
        }
    }

    private void RemoveLoot(ulong id)
    {
        GameObject lootObject = loots[id];
        Destroy(lootObject);
        loots.Remove(id);
    }
}
