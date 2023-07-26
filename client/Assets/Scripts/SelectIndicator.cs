using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectIndicator : MonoBehaviour
{
    [SerializeField]
    public GameObject cone;

    [SerializeField]
    GameObject arrow;

    [SerializeField]
    GameObject area;

    public float fov = 90f;
    public float angle = 0f;
    public float viewDistance = 50f;
    public int raycount = 50;
    public float angleInclease;

    public void Start()
    {
        CreateConeIndicator();
    }

    public void CreateConeIndicator()
    {
        angleInclease = fov / raycount;
        Mesh mesh = new Mesh();
        Vector3 origin = Vector3.zero;

        Vector3[] vertices = new Vector3[raycount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[raycount * 3];

        vertices[0] = origin;
        int vertexIndex = 1;
        int trianglesIndex = 0;

        for (int i = 0; i < raycount; i++)
        {
            Vector3 vertex = origin + GetVectorFromAngle(angle) * viewDistance;
            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[trianglesIndex + 0] = 0;
                triangles[trianglesIndex + 1] = vertexIndex - 1;
                triangles[trianglesIndex + 2] = vertexIndex;
                trianglesIndex += 3;
            }
            vertexIndex++;
            angle -= angleInclease;
        }

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        cone.GetComponent<MeshFilter>().mesh = mesh;
    }

    public Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    public void ActivateIndicator(UIIndicatorType indicatorType)
    {
        switch (indicatorType)
        {
            case UIIndicatorType.Cone:
                cone.SetActive(true);
                break;
            case UIIndicatorType.Arrow:
                arrow.SetActive(true);
                break;
            case UIIndicatorType.Area:
                area.SetActive(true);
                break;
        }
    }
}
