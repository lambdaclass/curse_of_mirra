using UnityEngine;

public class SafeZone : MonoBehaviour
{
    [SerializeField]
    GameObject map;

    [SerializeField]
    GameObject zoneLimit;

    private void Update()
    {
        float radius = Utils.transformBackendRadiusToFrontendRadius(
            SocketConnectionManager.Instance.playableRadius
        );

        Vector3 center = Utils.transformBackendPositionToFrontendPosition(
            SocketConnectionManager.Instance.shrinkingCenter
        );

        Material mapMaterial = map.GetComponent<Renderer>().material;
        mapMaterial.SetVector("_Center", center);
        mapMaterial.SetFloat("_Distance", radius / 2);

        float radiusCorrected = radius + radius * .007f;
        zoneLimit.transform.position = new Vector3(center.x, 42f, center.z);
        zoneLimit.transform.localScale = new Vector3(radiusCorrected, 50f, radiusCorrected);
    }
}
