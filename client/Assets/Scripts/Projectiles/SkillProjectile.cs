using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class SkillProjectile : MonoBehaviour
{
    [SerializeField]
    public ProjectileInfo projectileInfo;

    [SerializeField]
    GameObject projectileElement;

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer != 6)//Projectile Obstacle layer
            return;

        ProcessCollision();
    }
}
