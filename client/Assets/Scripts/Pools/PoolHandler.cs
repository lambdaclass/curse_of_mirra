using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.VFX;

public class PoolHandler : MonoBehaviour
{
    public List<MMSimpleObjectPooler> objectPoolerList;

    HashSet<SkillInfo> poolSkillsInfo;

    public Dictionary<int, (GameObject vfx, List<string> effects)> poolsFeedbacks = new Dictionary<int, (GameObject, List<string>)>();

    private readonly Color PURPLE = new Color(1f, 0f, .68f, 0f);
    private readonly Color LILE = new Color(.21f, .20f, .67f, 0f);

    private readonly Color ORIGINAL_COLOR_A = new Color(.3372549f, .172549f, .3803922f, 0f);
    private readonly Color ORIGINAL_COLOR_B = new Color(.2704863f, .8907394f, .8487674f, 0f);

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
        SpawnPools(poolsStates);
        HandlePoolsEffects(poolsStates);
    }

    private void ClearPools(List<Entity> poolsStates)
    {
        foreach (int poolId in poolsFeedbacks.Keys.ToList())
        {
            if (!poolsStates.Exists(x => (int)x.Id == poolId))
            {
                // is this line needed?
                // poolsGameObjects[poolId].GetComponent<SkillProjectile>().Remove();

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

                GameObject poolFeedback = InstantiatePool(
                    info.poolPrefab,
                    new Vector3(backToFrontPosition[0], 3f, backToFrontPosition[2])
                );

                poolsFeedbacks.Add((int)poolState.Id, (poolFeedback, new List<string>()));
            }
        }
    }

    private void HandlePoolsEffects(List<Entity> poolsStates)
    {
        // First iteration, hardcoded to work only with Valtimer's ultimate
        foreach(Entity poolState in poolsStates)
        {
            var poolFeedback = poolsFeedbacks[(int)poolState.Id];
            var newEffects = poolState.Pool.Effects.Select(effect => effect.Name).Where(effect => effect == "buff_singularity").Except(poolFeedback.effects);

            foreach(string newEffect in newEffects)
            {
                VisualEffect poolVFX = poolFeedback.vfx.GetComponentInChildren<VisualEffect>();
                poolFeedback.effects.Add(newEffect);
            }
        }
    }

    private IEnumerator BuffSingularity(VisualEffect poolVFX)
    {
        Debug.Log("buff singularity");
        poolVFX.SetVector4("Color A", PURPLE);
        poolVFX.SetVector4("Color B", LILE);

        yield return new WaitForSeconds(1f);

        poolVFX.SetVector4("Color A", ORIGINAL_COLOR_A);
        poolVFX.SetVector4("Color B", ORIGINAL_COLOR_B);
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
