using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

public class ProjectileHandler : MonoBehaviour
{
    MMSimpleObjectPooler objectPooler;

    public void SetProjectilePrefab()
    {
        objectPooler = Utils.SimpleObjectPooler("ShootPooler", transform.parent, gameObject.name);
        Debug.Log(transform.name);
    }

    public GameObject InstanceShoot(float direction)
    {
        GameObject Shoot = objectPooler.GetPooledGameObject();
        Shoot.SetActive(true);
        Shoot.transform.position = transform.position;
        Shoot.transform.rotation = Quaternion.Euler(0, direction, 0);

        return Shoot;
    }

    public void ShootLaser(GameObject projectile, Vector3 position)
    {
        projectile.transform.position = position;
    }

    public void LaserCollision(GameObject projectileToDestroy, string resourceName)
    {
        projectileToDestroy.SetActive(false);
        GameObject ShootFeedback =
            Instantiate(Resources.Load(resourceName, typeof(GameObject))) as GameObject;
        Destroy(ShootFeedback, 1f);
        ShootFeedback.transform.position = projectileToDestroy.transform.position;
    }

    public void LaserDisappear(GameObject projectileToDestroy)
    {
        projectileToDestroy.SetActive(false);
    }
}
