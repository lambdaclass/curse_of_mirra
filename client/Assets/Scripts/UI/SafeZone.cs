using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;

public class SafeZone : MonoBehaviour
{
    [SerializeField]
    GameObject smoke;

    [SerializeField]
    float smokeSize = 40f;

    float previusRadius = Mathf.Infinity;

    const float mapRadius = 50;
    Vector3 center = new Vector3(0, 1, 0);

    void Update()
    {
        // We have a difference of x100 between the backend and frontend values
        float radius = GameServerConnectionManager.Instance.playableRadius / 100;

        if (radius != 0)
        {
            // Vector3 center = Utils.transformBackendOldPositionToFrontendPosition(
            //     GameServerConnectionManager.Instance.shrinkingCenter
            // );
            // float radius = Utils.transformBackendRadiusToFrontendRadius(
            //     GameServerConnectionManager.Instance.playableRadius
            // );
            if ((radius < previusRadius) && (radius <= previusRadius - (smokeSize * 1.5)))
            {
                GenerateSmokeRing(radius, center);
                previusRadius = radius;
            }
        }
    }

    void GenerateSmokeRing(float radius, Vector3 center)
    {
        float perimeter = 2.0f * Mathf.PI * (radius);
        int totalSmokes = Mathf.RoundToInt(perimeter / smokeSize);

        for (int i = 0; i < totalSmokes; i++)
        {
            float angle = i * (360 / totalSmokes + 1);

            Vector3 pos = CalculatePositionInCircle(center, radius, angle);
            if (Mathf.Abs(pos.x) < mapRadius && Mathf.Abs(pos.z) < mapRadius)
            {
                Instantiate(smoke, pos, smoke.transform.rotation);
            }
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
