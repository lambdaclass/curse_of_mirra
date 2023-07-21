using UnityEngine;

public class SafeZone : MonoBehaviour
{
    private const float MAX_SAFE_ZONE_RADIUS = 200f;
    private const float MAX_SERVER_SAFE_ZONE_RADIUS = 5000f;
    private float safeZoneRadius;
    private SpriteMask safeZone;

    private void Awake()
    {
        safeZone = GetComponentInChildren<SpriteMask>();
        safeZoneRadius = MAX_SAFE_ZONE_RADIUS;
    }

    private void Update()
    {
        var radius = Utils.transformBackendRadiusToFrontendRadius(
            SocketConnectionManager.Instance.playableRadius
        );
        safeZone.transform.localScale = new Vector3(radius, radius, 2);
        var center = Utils.transformBackendPositionToFrontendPosition(
            SocketConnectionManager.Instance.shrinkingCenter
        );
        // 3.3f is the height of the safe zone in the scene.
        // TODO: Remove it when we improve the implementation of the damage area
        center.y += 3.3f;
        safeZone.transform.position = center;
    }
}
