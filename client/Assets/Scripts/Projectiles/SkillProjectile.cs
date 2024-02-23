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

    [SerializeField] public GameObject trail;

    public bool isTrailEnabled = true;

    public void UpdatePosition(Vector3 position)
    {
        if(!isUpdatingPosition)
            return;

        transform.position = position;
    }

    public void ProcessCollision()
    {
        // print(gameObject.activeSelf);
        // this.isTrailEnabled = trail != null;
        // // if(!gameObject.activeSelf)
        // //     return;

        // // if(removeTrailSoftCor != null)
        // //     return;

        // // isUpdatingPosition = false;
        // projectileElement?.SetActive(false);
        // // removeTrailSoftCor = removeTrailSoft();
        // // StartCoroutine(removeTrailSoftCor);

        // GameObject feedback = Instantiate(
        //     projectileInfo.projectileFeedback,
        //     transform.position,
        //     Quaternion.identity
        // );
        // var trailInstance = this.transform.Find(this.name + " Trail");
        // Destroy(trailInstance?.gameObject);
        // Destroy(feedback, 1f);

        // IEnumerator removeTrailSoft()
        // {
        //   if(trailRenderer == null)
        //   {
        //       gameObject.SetActive(false);
        //       yield break;
        //   }

        //   float cached_trail_time = trailRenderer.time;
        //   while(trailRenderer.time > 0.0f)
        //   {
        //       trailRenderer.time -= Time.deltaTime * 2;
        //       yield return null;
        //   }

        //   gameObject.SetActive(false);
        //   projectileElement?.SetActive(true);
        //   trailRenderer.time = cached_trail_time;
        //   removeTrailSoftCor = null;
        //   isUpdatingPosition = true;
        // }


        this.isTrailEnabled = trail != null;
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
        isUpdatingPosition = true;
        if (projectileElement)
        {
            projectileElement.SetActive(true);
        }
    }
}
