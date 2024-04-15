using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class DestroyableCrate : MonoBehaviour
{
    class CrateItem
    {
        public ulong id;
        public GameObject crateObject;
    }

    [SerializeField]
    CrateList cratesList;
    private List<CrateItem> crates = new List<CrateItem>();

    MMSimpleObjectPooler objectPooler;


    void Start()
    {
        for (int i = 0; i < cratesList.crateList.Count; i++)
        {
            this.objectPooler = Utils.SimpleObjectPooler(
                "CratePool",
                transform.parent.parent,
                cratesList.crateList[i].cratePrefab
            );
        }
    }

    private void MaybeAddLoot(Entity item)
    {
        if (!ExistInCrates(item.Id))
        {
            var position = Utils.transformBackendOldPositionToFrontendPosition(item.Position);
            position.y = 0;
            CrateItem LootItem = new CrateItem();
            LootItem.crateObject = objectPooler.GetPooledGameObject();
            LootItem.crateObject.transform.position = position;
            LootItem.crateObject.SetActive(true);
            LootItem.id = item.Id;
            crates.Add(LootItem);
        }
    }

    private void RemoveLoots(List<Entity> updatedCrates)
    {
        crates
            .ToList()
            .ForEach(crate =>
            {
                if (!updatedCrates.Exists(updateCrate => updateCrate.Id == crate.id))
                {
                    print("remover");
                    RemoveCrate(crate.id);
                }
                else
                {
                    print("existe crate");
                }
            });
    }

    private void RemoveCrate(ulong id)
    {
        GameObject lootObject = GetCrate(id).crateObject;

        lootObject.SetActive(false);
        RemoveById(id);
    }


    private bool ExistInCrates(ulong id)
    {
        return crates.Any(crate => crate.id == id);
    }

    private void RemoveById(ulong id)
    {
        CrateItem toRemove = crates.Find(crate => crate.id == id);
        crates.Remove(toRemove);
    }

    private CrateItem GetCrate(ulong id)
    {
        return crates.Find(crate => crate.id == id);
    }

    public void HandleCrateHealth(List<Entity> crateUpdate)
    {
        crates.ForEach(crate =>
        {
            Entity updatedCrate = crateUpdate.Find(updateCrate => updateCrate.Id == crate.id);
            Health healthComponent = crate.crateObject.GetComponent<Health>();
            if (updatedCrate.Crate.Health != healthComponent.CurrentHealth)
            {
                print("cambiar vida");
                healthComponent.SetHealth(updatedCrate.Crate.Health);
            }
        });
    }

    public void UpdateCrates()
    {
        List<Entity> updatedCrates =
            GameServerConnectionManager.Instance.gameCrates.Where(crate => crate.Crate.Status != CrateStatus.Destroyed).ToList();
        RemoveLoots(updatedCrates);
        HandleCrateHealth(updatedCrates);
        updatedCrates.ForEach(update_crate =>
        {
            MaybeAddLoot(update_crate);
        }
        );
    }

}
