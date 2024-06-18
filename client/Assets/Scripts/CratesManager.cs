using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class CratesManager : MonoBehaviour
{
    [SerializeField]
    CrateList cratesList;
    List<CrateItem> crates = new List<CrateItem>();
    MMSimpleObjectPooler objectPooler;
    const float openedCrateTime = 1.5f;

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

    private void MaybeAddCrate(Entity item)
    {
        if (!ExistInCrates(item.Id))
        {
            CrateItem crate = objectPooler.GetPooledGameObject().GetComponent<CrateItem>();

            crate.Initialize(item);

            crates.Add(crate);
        }
    }

    private void CleanUpCrates(List<Entity> updatedCrates)
    {
        List<CrateItem> cratesCopy = new List<CrateItem>(crates);

        foreach (var crate in cratesCopy)
        {
            if (!updatedCrates.Exists(updateCrate => updateCrate.Id == crate.serverId))
            {
                CrateItem crateItem = GetCrate(crate.serverId);
                StartCoroutine(ReturnItemToPooler(crateItem));
                RemoveById(crateItem.serverId);
            }
        }
    }

    IEnumerator ReturnItemToPooler(CrateItem crateItem)
    {
        crateItem.ExecuteOpenedFeedback();
        yield return new WaitForSeconds(openedCrateTime);
        crateItem.gameObject.SetActive(false);
    }

    private bool ExistInCrates(ulong id)
    {
        foreach (var crate in crates)
        {
            if (crate.serverId == id)
            {
                return true;
            }
        }
        return false;
    }

    private void RemoveById(ulong id)
    {
        CrateItem toRemove = crates.Find(crate => crate.serverId == id);
        crates.Remove(toRemove);
    }

    private CrateItem GetCrate(ulong id)
    {
        return crates.Find(crate => crate.serverId == id);
    }

    public void HandleCrateHealth(List<Entity> crateUpdate)
    {
        crates.ForEach(crate =>
        {
            Entity updatedCrate = crateUpdate.Find(updateCrate => updateCrate.Id == crate.serverId);
            crate.UpdateHealth(updatedCrate.Crate.Health);
        });
    }
}
