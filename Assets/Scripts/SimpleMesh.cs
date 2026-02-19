using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]

public class SimpleMesh : MonoBehaviour
{
    private UnityEngine.Mesh mesh;

    [SerializeField] Vector3[] newVertices;
    [SerializeField] Vector2[] newUV;
    [SerializeField] int[] newTriangles;

    private void GenerateSimpleSquareMesh()
    {
        //Create a mesh with the vertices: (0,0) (1,0) (0,1) (1,1)
        if (mesh == null)
        {
            mesh = new UnityEngine.Mesh();
        }
        else mesh.Clear();

        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(0,0); //No z-coordinate
        vertices[1] = new Vector3(1,0);
        vertices[2] = new Vector3(0,1);
        vertices[3] = new Vector3(1,1);


        int[] triangles = new int[6];
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 3;

        triangles[3] = 0;
        triangles[4] = 3;
        triangles[5] = 2;

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().sharedMesh = mesh;

    }

    private void OnDrawGizmos()
    {
        GenerateSimpleSquareMesh();
    }

}

