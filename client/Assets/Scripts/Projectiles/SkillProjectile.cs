using UnityEngine;
using MoreMountains.Tools;

public class SkillProjectile : MonoBehaviour
{
    [SerializeField]
    public ProjectileInfo projectileInfo;

    public void UpdateProjectilePosition(Vector3 position)
    {
        transform.position = position;
    }

    public void ProcessProjectileCollision()
    {
        gameObject.SetActive(false);
        GameObject feedback = Instantiate(
            projectileInfo.projectileFeedback,
            transform.position,
            Quaternion.identity
        );
        Destroy(feedback, 1f);
    }

    public void ClearProjectile()
    {
        gameObject.SetActive(false);
    }
}
