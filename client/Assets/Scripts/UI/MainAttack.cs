using MoreMountains.Tools;
using UnityEngine;

public class MainAttack : MoreMountains.TopDownEngine.CharacterAbility
{
    MMSimpleObjectPooler objectPooler;

    protected override void Initialization()
    {
        base.Initialization();
        InitializeObjectPooler();
    }

    public override void ProcessAbility()
    {
        base.ProcessAbility();
    }

    public GameObject InstanceShoot(float direction)
    {
        GameObject HackShoot = objectPooler.GetPooledGameObject();
        HackShoot.SetActive(true);
        HackShoot.transform.position = transform.position;
        HackShoot.transform.rotation = Quaternion.Euler(0, direction, 0);

        return HackShoot;
    }

    public void ShootLaser(GameObject projectile, Vector3 position)
    {
        projectile.transform.position = position;
    }

    public void LaserCollision(GameObject projectileToDestroy)
    {
        projectileToDestroy.SetActive(false);
        GameObject HackShootFeedback =
            Instantiate(Resources.Load("HackShootFeedback", typeof(GameObject))) as GameObject;
        Destroy(HackShootFeedback, 1f);
        HackShootFeedback.transform.position = projectileToDestroy.transform.position;
    }

    public void LaserDisappear(GameObject projectileToDestroy)
    {
        Destroy(projectileToDestroy.GetComponent<ShootHandler>().element);
        projectileToDestroy.SetActive(false);
    }

    private void InitializeObjectPooler()
    {
        GameObject objectPoolerGameObject = new GameObject();
        objectPoolerGameObject.name = "HackShootPooler";
        objectPoolerGameObject.transform.parent = this.transform;
        MMSimpleObjectPooler objectPooler =
            objectPoolerGameObject.AddComponent<MMSimpleObjectPooler>();
        objectPooler.GameObjectToPool =
            Resources.Load("HackShoot", typeof(GameObject)) as GameObject;
        objectPooler.PoolSize = 10;
        objectPooler.NestWaitingPool = true;
        objectPooler.MutualizeWaitingPools = true;
        objectPooler.PoolCanExpand = true;
        objectPooler.FillObjectPool();
        this.objectPooler = objectPooler;
    }
}
