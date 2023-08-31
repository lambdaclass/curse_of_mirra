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
            gameObject
        );
    }

    public GameObject InstanceProjectile(float angle)
    {
        GameObject projectileFromPooler = objectPooler.GetPooledGameObject();
        projectileFromPooler.SetActive(true);
        projectileFromPooler.transform.position = transform.position;
        projectileFromPooler.transform.rotation = Quaternion.Euler(0, angle, 0);

        return projectileFromPooler;
    }

    public void UpdateProjectilePosition(Vector3 position)
    {
        transform.position = position;
    }

    public void ProcessProjectilesCollision()
    {
        gameObject.SetActive(false);
        GameObject feedback = Instantiate(
            projectileInfo.projectileFeedback,
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
