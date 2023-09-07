using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using System.Linq;

public class ProjectileHandler : MonoBehaviour
{
    public List<MMSimpleObjectPooler> objectPoolerList;

    public void CreateProjectilesPoolers(HashSet<SkillInfo> skillInfoSet)
    {
        objectPoolerList = new List<MMSimpleObjectPooler>();
        foreach (SkillInfo skillInfo in skillInfoSet)
        {
            GameObject projectileFromSkill = skillInfo.projectilePrefab;
            MMSimpleObjectPooler objectPooler = Utils.SimpleObjectPooler(
                projectileFromSkill.name + "Pooler",
                transform.parent,
                projectileFromSkill
            );
            objectPoolerList.Add(objectPooler);
        }
    }

    public GameObject InstanceProjectile(GameObject skillProjectile, float direction)
    {
        MMSimpleObjectPooler projectileFromPooler = objectPoolerList.Find(
            objectPooler => objectPooler.name.Contains(skillProjectile.name)
        );
        GameObject pooledGameObject = projectileFromPooler.GetPooledGameObject();
        pooledGameObject.SetActive(true);
        pooledGameObject.transform.position = transform.position;
        pooledGameObject.transform.rotation = Quaternion.Euler(0, direction, 0);

        return pooledGameObject;
    }
}
