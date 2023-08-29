using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

public class SkillProjectile : MonoBehaviour
{
    MMSimpleObjectPooler objectPooler;

    [SerializeField]
    public ProjectileInfo projectileInfo;

    public void SetProjectilePooler()
    {
        objectPooler = Utils.SimpleObjectPooler(
            gameObject.name + "Pooler",
            transform.parent,
            gameObject.name
        );
    }

    public GameObject InstanceProjectile(float direction)
    {
        GameObject projectile = objectPooler.GetPooledGameObject();
        projectile.SetActive(true);
        projectile.transform.position = transform.position;
        projectile.transform.rotation = Quaternion.Euler(0, direction, 0);

        return projectile;
    }

    public void UpdateProjectilePosition(Vector3 position)
    {
        transform.position = position;
    }

    public void ProjectileCollision(string resourceName)
    {
        gameObject.SetActive(false);
        GameObject projectileFeedback =
            Instantiate(Resources.Load(resourceName, typeof(GameObject))) as GameObject;
        Destroy(projectileFeedback, 1f);
        projectileFeedback.transform.position = transform.position;
    }

    public void ProjectileDisappear()
    {
        gameObject.SetActive(false);
    }
}
