using System.Collections;
using UnityEngine;

public class SkillProjectile : MonoBehaviour
{
    [SerializeField]
    public ProjectileInfo projectileInfo;

    [SerializeField]
    GameObject projectileElement;

    [SerializeField] public GameObject trail;

    public bool isTrailEnabled = true;

    public void UpdatePosition(Vector3 position)
    {
        transform.position = position;
    }

    public void ProcessCollision()
    {
        this.isTrailEnabled = true;
        gameObject.SetActive(false);
        GameObject feedback = Instantiate(
            projectileInfo.projectileFeedback,
            transform.position,
            Quaternion.identity
        );
        var trailInstance = this.transform.Find(this.name + " Trail");
        Destroy(trailInstance?.gameObject);
        Destroy(feedback, 1f);
    }

    public void Remove()
    {
        if (projectileElement)
        {
            projectileElement.SetActive(false);
        }
        StartCoroutine(RestoreToPool());
    }

    private IEnumerator RestoreToPool()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
        if (projectileElement)
        {
            projectileElement.SetActive(true);
        }
    }
}
