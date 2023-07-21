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
        safeZone.transform.localScale = new Vector3(radius, radius, 1);
        var center = Utils.transformBackendPositionToFrontendPosition(
            SocketConnectionManager.Instance.shrinkingCenter
        );
        safeZone.transform.position = center;
    }
}
