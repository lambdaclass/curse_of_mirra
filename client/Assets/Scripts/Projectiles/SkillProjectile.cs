using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

public class SkillProjectile : MonoBehaviour
{
    MMSimpleObjectPooler objectPooler;

    [SerializeField]
    public ProjectileInfo projectileInfo;

    public void CreateProjectilePooler()
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

    public void ProcessProjectilesCollision(GameObject projectileFeedback)
    {
        gameObject.SetActive(false);
        GameObject feedback = Instantiate(
            projectileFeedback,
            transform.position,
            Quaternion.identity
        );
        Destroy(feedback, 1f);
    }

    public void ClearProjectiles()
    {
        gameObject.SetActive(false);
    }
}
