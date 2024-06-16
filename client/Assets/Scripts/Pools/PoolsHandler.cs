using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.VFX;

public class PoolsHandler : MonoBehaviour
{
    private Dictionary<string, MMSimpleObjectPooler> poolsPoolers;

    HashSet<SkillInfo> poolSkillsInfo;

    public Dictionary<int, PoolSkill> poolsFeedbacks = new Dictionary<int, PoolSkill>();

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
                    .Where(skill => skill.poolPrefab != null)
            );
        }
        CreatePoolsPoolers(poolSkillsInfo);
    }

    public void CreatePoolsPoolers(HashSet<SkillInfo> skillInfoSet)
    {
        poolsPoolers = new Dictionary<string, MMSimpleObjectPooler>();
        foreach (SkillInfo skillInfo in skillInfoSet)
        {
            MMSimpleObjectPooler poolsPooler = Utils.SimpleObjectPooler(
                skillInfo.name + "_Pooler",
                transform.parent,
                skillInfo.poolPrefab
            );
            poolsPoolers.Add($"{skillInfo.name}_Pooler", poolsPooler);
        }
    }

    public void UpdatePoolsActions()
    {
        List<Entity> poolsStates = GameServerConnectionManager.Instance.gamePools;
        ClearPools(poolsStates);
        SpawnPools(poolsStates);
        HandlePoolsEffects(poolsStates);
    }

    private void ClearPools(List<Entity> poolsStates)
    {
        foreach (int poolId in poolsFeedbacks.Keys.ToList())
        {
            if (!poolsStates.Exists(x => (int)x.Id == poolId))
            {
                poolsFeedbacks[poolId].TurnOff();
                poolsFeedbacks.Remove(poolId);
            }
        }
    }

    private void SpawnPools(List<Entity> poolsStates)
    {
        foreach(Entity poolState in poolsStates)
        {
            if(!poolsFeedbacks.Keys.Contains((int)poolState.Id))
            {

                Vector3 backToFrontPosition = Utils.transformBackendOldPositionToFrontendPosition(
                    poolState.Position
                );

                string poolSkillKey = poolState.Pool.SkillKey;
                ulong skillOwner = poolState.Pool.OwnerId;

                SkillInfo info = poolSkillsInfo
                    .Where(el => el.poolSkillKey == poolSkillKey && el.ownerId == skillOwner)
                    .FirstOrDefault();

                PoolSkill poolFeedback = InstantiatePool(
                    info.name,
                    new Vector3(backToFrontPosition[0], 3f, backToFrontPosition[2])
                );

                poolsFeedbacks.Add((int)poolState.Id, poolFeedback);
            }
        }
    }

    private void HandlePoolsEffects(List<Entity> poolsStates)
    {
        foreach(Entity poolState in poolsStates)
        {
            PoolSkill poolFeedback = poolsFeedbacks[(int)poolState.Id];
            poolFeedback.HandlePoolEffects(poolState.Pool.Effects);

            poolsFeedbacks[(int)poolState.Id] = poolFeedback;
        }
    }

    public PoolSkill InstantiatePool(string skillName, Vector3 initialPosition)
    {
        MMSimpleObjectPooler poolsPooler = poolsPoolers[$"{skillName}_Pooler"];
        GameObject pooledGameObject = poolsPooler.GetPooledGameObject();
        pooledGameObject.transform.position = initialPosition;
        pooledGameObject.SetActive(true);

        return pooledGameObject.GetComponent<PoolSkill>();
    }
}
