using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;

public class SafeZone : MonoBehaviour
{
    [SerializeField]
    GameObject mesh;

    [SerializeField]
    public float div = 3.333f;

    float previusRadius = 0.0f;

    void Start()
    {
        mesh = Instantiate(mesh, Vector3.zero, Quaternion.identity);
        StartCoroutine(SetupCollitions());
    }

    IEnumerator SetupCollitions()
    {
        yield return new WaitUntil(() => SocketConnectionManager.Instance.players.Count != 0);
        List<SphereCollider> colliders = GameObject.FindObjectsOfType<SphereCollider>().ToList();
        Utils
            .GetAllCharacters()
            .ForEach(character =>
            {
                colliders.ForEach(
                    collider =>
                        Physics.IgnoreCollision(
                            collider,
                            character.GetComponent<CharacterController>()
                        )
                );
            });
    }

    public void ActivateParticleEffects(float radius, Vector3 center)
    {
        if (previusRadius != radius) //for call optimization
        {
            mesh.transform.position = new Vector3(center.x, 1f, center.z);
            mesh.transform.localScale = new Vector3(radius, 10, radius);
            previusRadius = radius;
        }
    }

    private void Update()
    {
        float radius = Utils.transformBackendRadiusToFrontendRadius(
            SocketConnectionManager.Instance.playableRadius
        );

        Vector3 center = Utils.transformBackendPositionToFrontendPosition(
            SocketConnectionManager.Instance.shrinkingCenter
        );

        float radiusCorrected = radius + radius * .007f;

        ActivateParticleEffects(radius / div, new Vector3(center.x, 1f, center.z));
    }
}
