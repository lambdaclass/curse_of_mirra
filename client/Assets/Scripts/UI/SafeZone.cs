using UnityEngine;
using UnityEngine.VFX;

public class SafeZone : MonoBehaviour
{
    [SerializeField]
    GameObject map;

    [SerializeField]
    GameObject zoneLimit;

    [SerializeField]
    GameObject particle;

    GameObject part = new GameObject();

    float intialRadius = 0.0f;

    bool first = false;

    private void Update()
    {
        float radius = Utils.transformBackendRadiusToFrontendRadius(
            SocketConnectionManager.Instance.playableRadius
        );

        Vector3 center = Utils.transformBackendPositionToFrontendPosition(
            SocketConnectionManager.Instance.shrinkingCenter
        );

        float radiusCorrected = radius + radius * .007f;

        if (radius == 200 && !first)
        {
            intialRadius = radius;
            part = Instantiate(particle);
            part.GetComponent<VisualEffect>().SetFloat("CircleRadius", radiusCorrected);
            part.transform.position = new Vector3(center.x, 1f, center.z);
            first = true;
        }

        if (intialRadius - radius == 9)
        {
            intialRadius = radius;
            part = Instantiate(particle);
            part.GetComponent<VisualEffect>().SetFloat("CircleRadius", radiusCorrected);
            part.transform.position = new Vector3(center.x, 1f, center.z);
        }
    }
}
