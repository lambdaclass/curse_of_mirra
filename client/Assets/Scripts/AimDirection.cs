using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimDirection : MonoBehaviour
{
    [SerializeField]
    Color32 characterFeedbackColor = new Color32(255, 255, 255, 255);

    [SerializeField]
    public GameObject cone;

    [SerializeField]
    GameObject arrow;

    [SerializeField]
    GameObject arrowHead;

    [SerializeField]
    GameObject area;

    [SerializeField]
    GameObject surface;
    private Vector3 initialPosition;

    UIIndicatorType activeIndicator = UIIndicatorType.None;

    public float fov = 90f;
    public float angle = 0f;
    public float viewDistance = 50f;
    public int rayCount = 50;
    public float angleIncrease;

    private float scaleZ = 0.05f;

    public void InitIndicator(Skill skill, Color32 color)
    {
        // TODO: Add the spread area (angle) depending of the skill.json
        viewDistance = skill.GetSkillRadius();
        fov = skill.GetIndicatorAngle();
        activeIndicator = skill.GetIndicatorType();
        characterFeedbackColor = color;
        initialPosition = transform.localPosition;

        SetColor(color);

        if (skill.GetIndicatorType() == UIIndicatorType.Arrow)
        {
            float scaleX = skill.GetArroWidth();
            float scaleY = skill.GetSkillRadius();
            arrow.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
            arrow.transform.localPosition = new Vector3(0, -scaleY / 2, 0);
        }

        surface.transform.localScale = new Vector3(viewDistance * 2, viewDistance * 2, scaleZ);
        surface.GetComponentInChildren<Renderer>().material.color = new Color32(255, 255, 255, 100);
        surface.SetActive(skill.IsSelfTargeted());
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
        angleIncrease = fov / rayCount;
        Mesh mesh = new Mesh();
        Vector3 origin = Vector3.zero;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;
        int vertexIndex = 1;
        int trianglesIndex = 0;

        for (int i = 0; i < rayCount; i++)
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
            angle -= angleIncrease;
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

    public void DeactivateIndicator()
    {
        surface.SetActive(false);

        switch (activeIndicator)
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
        Reset();
    }

    private void Reset()
    {
        transform.localPosition = initialPosition;
    }

    public void CancelableFeedback(bool cancelable)
    {
        Color32 newColor = cancelable ? new Color32(255, 0, 0, 255) : characterFeedbackColor;
        SetColor(newColor);

        newColor.a = 60;
        surface.GetComponentInChildren<Renderer>().material.color = newColor;
    }

    public void SetColor(Color32 color)
    {
        switch (activeIndicator)
        {
            case UIIndicatorType.Cone:
                color.a = 60;
                cone.GetComponent<Renderer>().material.SetColor("_TopColor", color);
                break;
            case UIIndicatorType.Arrow:
                arrow.GetComponent<Renderer>().material.color = color;
                arrowHead.GetComponent<Renderer>().material.color = color;
                break;
            case UIIndicatorType.Area:
                area.GetComponent<Renderer>().material.color = color;
                break;
        }
    }
}
