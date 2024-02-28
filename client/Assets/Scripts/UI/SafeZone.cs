using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;

public class SafeZone : MonoBehaviour
{
    [SerializeField]
    float smokeSize = 40f;

    [SerializeField]
    VisualEffect damageFog = null;

    float previusRadius = Mathf.Infinity;
    private string DAMAGE_ZONE_PROPERTY = "DamageZonePercent";

    const float mapRadius = 50;
    Vector3 center = new Vector3(0, 1, 0);
    float currentRadius = 0;

    void Update()
    {
        // We have a difference of x100 between the backend and frontend values
        float radius = GameServerConnectionManager.Instance.playableRadius / 100;
        bool zoneEnabled = GameServerConnectionManager.Instance.zoneEnabled;
        damageFog.enabled = zoneEnabled;

        if (radius != 0 && zoneEnabled && (int)currentRadius != radius)
        {
            damageFog.SetFloat(DAMAGE_ZONE_PROPERTY, 1 - (radius / mapRadius));
            currentRadius = radius;
        }
    }

    Vector3 CalculatePositionInCircle(Vector3 center, float radius, float angleDeg)
    {
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(angleDeg * Mathf.Deg2Rad);
        pos.y = 1f;
        pos.z = center.z + radius * Mathf.Cos(angleDeg * Mathf.Deg2Rad);
        return pos;
    }
}
