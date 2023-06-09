using UnityEngine;

public class MainAttack : MoreMountains.TopDownEngine.CharacterAbility
{
    protected override void Initialization()
    {
        base.Initialization();
    }
    public override void ProcessAbility()
    {
        base.ProcessAbility();
    }
    public GameObject InstanceShoot(float direction)
    {
        GameObject HackShoot = Instantiate(Resources.Load("HackShoot", typeof(GameObject))) as GameObject;
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
        Destroy(projectileToDestroy);
        GameObject HackShootFeedback = Instantiate(Resources.Load("HackShootFeedback", typeof(GameObject))) as GameObject;
        Destroy(HackShootFeedback, 1f);
        HackShootFeedback.transform.position = projectileToDestroy.transform.position;
    }
    public void LaserDisappear(GameObject projectileToDestroy)
    {
        Destroy(projectileToDestroy.GetComponent<ShootHandler>().element);
        Destroy(projectileToDestroy, 0.1f);
    }
}
