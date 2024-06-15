using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.Tools;
using UnityEngine;

public class PoolHandler : MonoBehaviour
{
    public List<MMSimpleObjectPooler> objectPoolerList;

    public Dictionary<int, GameObject> poolsGameObjects = new Dictionary<int, GameObject>();

    public void UpdatePools()
    {
        List<Entity> poolsStates = GameServerConnectionManager.Instance.gamePools;
        ClearPools(poolsStates);
    }

    public void CreatePoolsPoolers(HashSet<SkillInfo> skillInfoSet)
    {
        objectPoolerList = new List<MMSimpleObjectPooler>();
        foreach (SkillInfo skillInfo in skillInfoSet)
        {
            GameObject poolFromSkill = skillInfo.poolPrefab;
            MMSimpleObjectPooler objectPooler = Utils.SimpleObjectPooler(
                poolFromSkill.name + "Pooler",
                transform.parent,
                poolFromSkill
            );
            objectPoolerList.Add(objectPooler);
        }
    }

    public GameObject InstancePool(GameObject skillPool, Vector3 initialPosition)
    {
        MMSimpleObjectPooler projectileFromPooler = objectPoolerList.Find(
            objectPooler => objectPooler.name.Contains(skillPool.name)
        );
        GameObject pooledGameObject = projectileFromPooler.GetPooledGameObject();
        pooledGameObject.transform.position = initialPosition;
        pooledGameObject.SetActive(true);

        return pooledGameObject;
    }

    private void ClearPools(List<Entity> poolsStates)
    {
        foreach (int poolId in poolsGameObjects.Keys.ToList())
        {
            if (!poolsStates.Exists(x => (int)x.Id == poolId))
            {
                poolsGameObjects[poolId].GetComponent<SkillProjectile>().Remove();
                poolsGameObjects.Remove(poolId);
            }
        }
    }
}
