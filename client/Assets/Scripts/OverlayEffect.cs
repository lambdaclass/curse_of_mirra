using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class OverlayEffect : MonoBehaviour
{
    public Shader shader;
    public Material material;
    public Camera camera;
    public CommandBuffer commandBuffer;

    [Header("Settings")]
    public CameraEvent cameraEvent = CameraEvent.BeforeImageEffects;
    public int selectedGroupID = 1;

    List<SkinnedMeshRenderer> skinnedMeshFilter = new List<SkinnedMeshRenderer>();
    [Header("Objects")]
    public GameObject objectToHighlight;

    public void OnEnable()
    {
        if (camera == null)
            camera = Camera.main;

        shader = Shader.Find("Unlit/OverlayShader");
        material = new Material(shader);
        commandBuffer = new CommandBuffer();
        camera = Camera.main;
        camera.AddCommandBuffer(cameraEvent, commandBuffer);
    }

    public void OnDisable()
    {
        if (commandBuffer != null && camera != null)
        {
            camera.RemoveCommandBuffer(cameraEvent, commandBuffer);
            commandBuffer.Release();
            commandBuffer = null;
        }
    }

    void LateUpdate()
    {
        commandBuffer.Clear();
        commandBuffer.DrawAllMeshes(objectToHighlight, material, 0);
    }
}
