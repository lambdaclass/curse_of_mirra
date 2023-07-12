using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class OverlayEffect : MonoBehaviour
{
    public Shader shader;

    List<SkinnedMeshRenderer> skinnedMeshFilter = new List<SkinnedMeshRenderer>();

    [Header("Objects")]
    public GameObject objectToHighlight;

    public void OnEnable()
    {
        objectToHighlight.GetComponentsInChildren(skinnedMeshFilter);
        foreach (var meshFilter in skinnedMeshFilter)
        {
            meshFilter.GetComponent<Renderer>().material.shader = shader;
        }
    }

    public void OnDisable()
    {
        objectToHighlight.GetComponentsInChildren(skinnedMeshFilter);
        foreach (var meshFilter in skinnedMeshFilter)
        {
            meshFilter.GetComponent<Renderer>().material.shader = Shader.Find(
                "Universal Render Pipeline/Lit"
            );
        }
    }
}
