using UnityEngine;
using MoreMountains.Tools;

public class SkillProjectile : MonoBehaviour
{
    [SerializeField]
    public ProjectileInfo projectileInfo;

    public void UpdatePosition(Vector3 position)
    {
        transform.position = position;
    }

    public void ProcessCollision()
    {
        gameObject.SetActive(false);
        GameObject feedback = Instantiate(
            projectileInfo.projectileFeedback,
            transform.position,
            Quaternion.identity
        );
        Destroy(feedback, 1f);
    }

    public void Remove()
    {
        gameObject.SetActive(false);
    }
}
