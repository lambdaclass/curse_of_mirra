using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.Tools;
using UnityEngine;

public class PoolHandler : MonoBehaviour
{
    public List<MMSimpleObjectPooler> objectPoolerList;

    HashSet<SkillInfo> poolSkillsInfo;

    public Dictionary<int, GameObject> poolsGameObjects = new Dictionary<int, GameObject>();

    public IEnumerator SetUpPoolsSkills()
    {
        yield return new WaitUntil(() => GameServerConnectionManager.Instance.players.Count > 0);

        poolSkillsInfo = new HashSet<SkillInfo>();
        foreach (GameObject player in GameServerConnectionManager.Instance.players)
        {
            poolSkillsInfo.UnionWith(
                player
                    .GetComponents<Skill>()
                    .Select(skill => skill.GetSkillInfo())
                    .Where(skill => skill.hasSkillPool)
            );
        }
        CreatePoolsPoolers(poolSkillsInfo);
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

    public void UpdatePoolsActions()
    {
        List<Entity> poolsStates = GameServerConnectionManager.Instance.gamePools;
        ClearPools(poolsStates);
        UpdatePools(poolsStates);
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
        foreach(Entity poolState in poolsStates)
        {
            if(!poolsGameObjects.Keys.Contains((int)poolState.Id))
            {
                Vector3 backToFrontPosition = Utils.transformBackendOldPositionToFrontendPosition(
                    poolState.Position
                );

                string poolSkillKey = poolState.Pool.SkillKey;
                ulong skillOwner = poolState.Pool.OwnerId;

                SkillInfo info = poolSkillsInfo
                    .Where(el => el.poolSkillKey == poolSkillKey && el.ownerId == skillOwner)
                    .FirstOrDefault();

                GameObject poolFeedback = InstantiatePool(
                    info.poolPrefab,
                    new Vector3(backToFrontPosition[0], 3f, backToFrontPosition[2])
                );

                poolsGameObjects.Add((int)poolState.Id, poolFeedback);
            }
        }


        // if pools in the state have effect, react to those effects accordingly (DIVIDE IN ANOTHER METHOD?)
    }

    public GameObject InstantiatePool(GameObject skillPool, Vector3 initialPosition)
    {
        MMSimpleObjectPooler projectileFromPooler = objectPoolerList.Find(
            objectPooler => objectPooler.name.Contains(skillPool.name)
        );
        GameObject pooledGameObject = projectileFromPooler.GetPooledGameObject();
        pooledGameObject.transform.position = initialPosition;
        pooledGameObject.SetActive(true);

        return pooledGameObject;
    }
}
