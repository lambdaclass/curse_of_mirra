using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GenerateMeshCollider();
           
    }

    

 public void GenerateMeshCollider()
    {
        MeshCollider collider = GetComponent<MeshCollider>();

        
       Mesh mesh = new Mesh();
       this.GetComponent<LineRenderer>().BakeMesh(mesh, false);
       for(int i = 0; i < mesh.vertices.Length; i++){
          mesh.vertices[i].y *= 10;
       }
        collider.sharedMesh = mesh;
        collider.convex = true;
        this.GetComponent<LineRenderer>().enabled = false;
    }
}
