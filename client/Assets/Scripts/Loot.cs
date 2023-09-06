using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.Tools;
using UnityEngine;

public class Loot : MonoBehaviour
{
    [SerializeField]
    LootableList lootsList;

    private Dictionary<ulong, GameObject> loots = new Dictionary<ulong, GameObject>();

    MMSimpleObjectPooler objectPooler;

    void Start()
    {
        for (int i = 0; i < lootsList.LootList.Count; i++)
        {
            this.objectPooler = Utils.SimpleObjectPooler(
                "LootPool",
                transform.parent.parent,
                lootsList.LootList[i].lootPrefab
            );
        }
    }

    private void MaybeAddLoot(LootPackage loot)
    {
        if (!loots.ContainsKey(loot.Id))
        {
            var position = Utils.transformBackendPositionToFrontendPosition(loot.Position);
            position.y = 0;
            GameObject lootPoolObj = objectPooler.GetPooledGameObject();
            lootPoolObj.transform.position = position;
            lootPoolObj.name = loot.Id.ToString();
            lootPoolObj.transform.rotation = Quaternion.identity;
            lootPoolObj.SetActive(true);
            loots.Add(loot.Id, lootPoolObj);
        }
    }

    private void RemoveLoots(List<LootPackage> updatedLoots)
    {
        var idsToRemove = this.loots.Keys.Except(updatedLoots.Select(loot => loot.Id)).ToList();
        idsToRemove.ForEach(RemoveLoot);
    }

    private void RemoveLoot(ulong id)
    {
        GameObject lootObject = loots[id];
        lootObject.SetActive(false);
        loots.Remove(id);
    }

    public void UpdateLoots()
    {
        List<LootPackage> updatedLoots = SocketConnectionManager.Instance.updatedLoots;
        RemoveLoots(updatedLoots);
        updatedLoots.ForEach(MaybeAddLoot);
    }
}
