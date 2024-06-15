using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.Tools;
using UnityEngine;

public class PoolHandler : MonoBehaviour
{
    public List<MMSimpleObjectPooler> objectPoolerList;

    public Dictionary<int, GameObject> poolsGameObjects = new Dictionary<int, GameObject>();

    public void UpdatePoolsActions()
    {
        List<Entity> poolsStates = GameServerConnectionManager.Instance.gamePools;
        ClearPools(poolsStates);
        UpdatePools(poolsStates);
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
                // is this line needed?
                // poolsGameObjects[poolId].GetComponent<SkillProjectile>().Remove();

                poolsGameObjects.Remove(poolId);
            }
        }
    }

    private void UpdatePools(List<Entity> poolsStates)
    {
        // Find if there are pools that exist in state but not as a game object
        // if there are, create game objects for them

        // if pools in the state have effect, react to those effects accordingly
    }
}
