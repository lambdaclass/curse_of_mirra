using System;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;
public class DetectNearPlayer : MonoBehaviour
{
    public float radius = 3.14f;

    void Update()
    {
        ScanForTargets();
    }

    protected virtual void ScanForTargets()
    {
        float distanceToClosestTarget = Mathf.Infinity;
        GameObject nearestTarget = null;
        Collider[] nearby = GetOnlyPlayersColliders();
        foreach (var hitCollide in nearby)
        {
            float distance = Vector3.Distance(transform.position, hitCollide.transform.position);
            if (distance < distanceToClosestTarget)
            {
                distanceToClosestTarget = distance;
                nearestTarget = hitCollide.gameObject;
                print("Closes enemy from " + gameObject.name + " is " + nearestTarget.name + " at " + distanceToClosestTarget);
            }
        }
        if (nearestTarget != null)
        {
            Debug.DrawLine(transform.position, nearestTarget.transform.position);
        }
    }

    private Collider[] GetOnlyPlayersColliders()
    {
        return (Physics.OverlapSphere(transform.position, radius)).Where(c => c.CompareTag("Player") && c.GetComponent<Character>().PlayerID != (LobbyConnection.Instance.playerId).ToString()).ToArray();
    }


    void OnDrawGizmosSelected()
    {
        print("drawing");
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
