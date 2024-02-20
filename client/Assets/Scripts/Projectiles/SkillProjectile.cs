using System.Collections;
using UnityEngine;

public class SkillProjectile : MonoBehaviour
{
    [SerializeField]
    public ProjectileInfo projectileInfo;

    [SerializeField]
    GameObject projectileElement;

    [SerializeField]
    TrailRenderer trailRenderer;

    private IEnumerator removeTrailSoftCor = null;
    private bool isUpdatingPosition = true;

    public void UpdatePosition(Vector3 position)
    {
        if(!isUpdatingPosition)
            return;

        transform.position = position;
    }

    public void ProcessCollision()
    {
        if(!gameObject.activeSelf)
            return;

        if(removeTrailSoftCor != null)
            return;

        isUpdatingPosition = false;
        projectileElement?.SetActive(false);
        removeTrailSoftCor = removeTrailSoft();
        StartCoroutine(removeTrailSoftCor);

        GameObject feedback = Instantiate(
            projectileInfo.projectileFeedback,
            transform.position,
            Quaternion.identity
        );
        Destroy(feedback, 1f);

        IEnumerator removeTrailSoft()
        {
          if(trailRenderer == null)
          {
              gameObject.SetActive(false);
              yield break;
          }

          float cached_trail_time = trailRenderer.time;
          while(trailRenderer.time > 0.0f)
          {
              trailRenderer.time -= Time.deltaTime * 2;
              yield return null;
          }

          gameObject.SetActive(false);
          projectileElement?.SetActive(true);
          trailRenderer.time = cached_trail_time;
          removeTrailSoftCor = null;
          isUpdatingPosition = true;
        }
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
        isUpdatingPosition = true;
        if (projectileElement)
        {
            projectileElement.SetActive(true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer != 6) //Projectile Obstacle layer
            return;

        ProcessCollision();
    }
}
