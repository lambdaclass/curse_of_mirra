using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.Tools;
using UnityEngine;

public class Loot : MonoBehaviour
{
    class LootItem
    {
        public ulong id;
        public GameObject lootObject;
        public string type;
    }

    [SerializeField]
    LootableList lootsList;
    private List<LootItem> loots = new List<LootItem>();

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
        if (!ExistInLoots(loot.Id))
        {
            var position = Utils.transformBackendPositionToFrontendPosition(loot.Position);
            position.y = 0;
            LootItem LootItem = new LootItem();
            LootItem.lootObject = objectPooler.GetPooledGameObject();
            LootItem.lootObject.transform.position = position;
            LootItem.lootObject.name = loot.Id.ToString();
            LootItem.lootObject.transform.rotation = Quaternion.identity;
            LootItem.lootObject.SetActive(true);
            LootItem.id = loot.Id;
            LootItem.type = loot.LootType.ToString();
            loots.Add(LootItem);
        }
    }

    private void RemoveLoots(List<LootPackage> updatedLoots)
    {
        loots
            .ToList()
            .ForEach(loot =>
            {
                if (!updatedLoots.Exists(lootPackage => lootPackage.Id == loot.id))
                {
                    RemoveLoot(loot.id);
                }
            });
    }

    private void RemoveLoot(ulong id)
    {
        GameObject lootObject = GetLoot(id).lootObject;
        string type = GetLoot(id).type;

        Sound3DManager lootSoundManagerRef = lootObject.GetComponent<Sound3DManager>();
        lootSoundManagerRef.SetSfxSound(GetLootableByType(type).pickUpSound);
        lootSoundManagerRef.PlaySfxSound();

        lootObject.SetActive(false);
        RemoveById(id);
    }

    private bool ExistInLoots(ulong id)
    {
        return loots.Any(loot => loot.id == id);
    }

    private void RemoveById(ulong id)
    {
        LootItem toRemove = loots.Find(loot => loot.id == id);
        loots.Remove(toRemove);
    }

    private LootItem GetLoot(ulong id)
    {
        return loots.Find(loot => loot.id == id);
    }

    private Lootable GetLootableByType(string type)
    {
        return lootsList.LootList.Find(loot => loot.type == type);
    }

    public void UpdateLoots()
    {
        List<LootPackage> updatedLoots = SocketConnectionManager.Instance.updatedLoots;
        RemoveLoots(updatedLoots);
        updatedLoots.ForEach(MaybeAddLoot);
    }
}
