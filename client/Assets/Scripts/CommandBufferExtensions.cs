using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public static class CommandBufferExtensions
{
    private static List<SkinnedMeshRenderer> skinnedMeshFilter = new List<SkinnedMeshRenderer>();
    public static void DrawAllMeshes(this CommandBuffer cmd, GameObject gameObject, Material material, int pass)
    {
        skinnedMeshFilter.Clear();
        gameObject.GetComponentsInChildren(skinnedMeshFilter);

        foreach (var meshFilter in skinnedMeshFilter)
        {
            // Static objects may use static batching, preventing us from accessing their default mesh
            if (!meshFilter.gameObject.isStatic)
            {
                foreach (var skinnedMesh in skinnedMeshFilter)
                {
                    var mesh = skinnedMesh.sharedMesh;
                    // Render all submeshes
                    for (int i = 0; i < mesh.subMeshCount; i++)
                        cmd.DrawRenderer(skinnedMesh, material, i, pass);
                }

            }
        }
    }
}
