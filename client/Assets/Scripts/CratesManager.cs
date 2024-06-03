using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class CratesManager : MonoBehaviour
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

    private void MaybeAddCrate(Entity item)
    {
        if (!ExistInCrates(item.Id))
        {
            var position = Utils.transformBackendOldPositionToFrontendPosition(item.Position);
            position.y = 0;
            CrateItem crate = new CrateItem();
            crate.id = item.Id;

            crate.crateObject = objectPooler.GetPooledGameObject();
            crate.crateObject.transform.position = position;
            crate.crateObject.SetActive(true);
            crate.crateObject.GetComponent<Health>().CurrentHealth = item.Crate.Health;
            crate.crateObject.GetComponent<Health>().InitialHealth = item.Crate.Health;
            crate.crateObject.GetComponent<Health>().MaximumHealth = item.Crate.Health;

            crates.Add(crate);
        }
    }

    private void CleanUpCrates(List<Entity> updatedCrates)
    {
        List<CrateItem> cratesCopy = new List<CrateItem>(crates);

        foreach (var crate in cratesCopy)
        {
            if (!updatedCrates.Exists(updateCrate => updateCrate.Id == crate.id))
            {
                RemoveCrate(crate.id);
            }
        }
    }

    private void RemoveCrate(ulong id)
    {
        GameObject crateObject = GetCrate(id).crateObject;

        crateObject.SetActive(false);
        RemoveById(id);
    }

    private bool ExistInCrates(ulong id)
    {
        foreach (var crate in crates)
        {
            if (crate.id == id)
            {
                return true;
            }
        }
        return false;
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
                healthComponent.SetHealth(updatedCrate.Crate.Health);
            }
        });
    }

    public void UpdateCrates()
    {
        List<Entity> updatedCrates = new List<Entity>();

        foreach (var crate in GameServerConnectionManager.Instance.gameCrates)
        {
            if (crate.Crate.Status != CrateStatus.Destroyed)
            {
                updatedCrates.Add(crate);
            }
        }

        CleanUpCrates(updatedCrates);
        HandleCrateHealth(updatedCrates);

        foreach (var update_crate in updatedCrates)
        {
            MaybeAddCrate(update_crate);
        }
    }
}
