using UnityEngine;

public class SafeZone : MonoBehaviour
{
    // private const float MAX_SAFE_ZONE_RADIUS = 200f;
    // private const float MAX_SERVER_SAFE_ZONE_RADIUS = 5000f;
    // private float safeZoneRadius;
    // private SpriteMask safeZone;

    [SerializeField]
    GameObject map;

    [SerializeField]
    GameObject zoneLimit;

    private void Awake()
    {
        // safeZone = GetComponentInChildren<SpriteMask>();
        // safeZoneRadius = MAX_SAFE_ZONE_RADIUS;
    }

    private void Update()
    {
        float radius = Utils.transformBackendRadiusToFrontendRadius(
            SocketConnectionManager.Instance.playableRadius
        );
        // safeZone.transform.localScale = new Vector3(radius, radius, 2);
        Vector3 center = Utils.transformBackendPositionToFrontendPosition(
            SocketConnectionManager.Instance.shrinkingCenter
        );
        // 3.3f is the height of the safe zone in the scene.
        // TODO: Remove it when we improve the implementation of the damage area
        // center.y += 3.3f;
        // safeZone.transform.position = center;

        Material mapShaderMat = map.GetComponent<Renderer>().sharedMaterial;
        mapShaderMat.SetVector("_Center", center);
        mapShaderMat.SetFloat("_Distance", radius / 2);

        zoneLimit.transform.position = new Vector3(center.x, 42f, center.z);
        zoneLimit.transform.localScale = new Vector3(radius, 50f, radius);
    }
}
