using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimDirection : MonoBehaviour
{
    [SerializeField]
    public GameObject cone;

    [SerializeField]
    GameObject arrow;
    GameObject newArrow;

    [SerializeField]
    GameObject area;

    public float fov = 90f;
    public float angle = 0f;
    public float viewDistance = 50f;
    public int raycount = 50;
    public float angleInclease;

    public void InitIndicator(Skill skill)
    {
        // TODO: Add the spread area (amgle) depeding of the skill.json
        viewDistance = skill.GetSkillRadius();
        fov = skill.GetIndicatorAngle();

        if (skill.GetIndicatorType() == UIIndicatorType.Arrow)
        {
            float scaleX = skill.GetArroWidth();
            float scaleY = skill.GetSkillRadius();
            arrow.transform.localScale = new Vector3(scaleX, scaleY, 0.05f);
            arrow.transform.localPosition = new Vector3(0, -scaleY / 2, 0);
        }
    }

    public void Rotate(float x, float y, Skill skill)
    {
        var result = Mathf.Atan(x / y) * Mathf.Rad2Deg;
        if (y >= 0)
        {
            result += 180f;
        }
        transform.rotation = Quaternion.Euler(
            90f,
            result,
            skill.GetIndicatorType() == UIIndicatorType.Cone
                ? -(180 - skill.GetIndicatorAngle()) / 2
                : 0
        );
    }

    public void SetConeIndicator()
    {
        angle = 0;
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

    public void DeactivateIndicator(UIIndicatorType indicatorType)
    {
        switch (indicatorType)
        {
            case UIIndicatorType.Cone:
                cone.SetActive(false);
                break;
            case UIIndicatorType.Arrow:
                arrow.SetActive(false);
                break;
            case UIIndicatorType.Area:
                area.SetActive(false);
                break;
        }
    }
}
