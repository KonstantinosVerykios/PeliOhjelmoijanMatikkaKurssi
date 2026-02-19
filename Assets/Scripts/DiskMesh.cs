using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]

public class DiskMesh : MonoBehaviour
{
    private UnityEngine.Mesh mesh;

    [Range(0.1f, 10f)]
    public float Radius = 1.0f;

    [Range(3, 32)]
    public int Segments = 8;

    private void GenerateSimpleSquareMesh()
    {
        //Create a mesh with the vertices: (0,0) (1,0) (0,1) (1,1)
        if (mesh == null)
        {
            mesh = new UnityEngine.Mesh();
        }
        else mesh.Clear();

        Vector3[] vertices = new Vector3[Segments + 1];
        vertices[0] = Vector3.zero;

        float deltaAngle = 2.0f * Mathf.PI / Segments;

        for(int i = 0; i < Segments; i++)
        {
            float currentAngle = deltaAngle * i;

            float x = Mathf.Cos(currentAngle);
            float y = Mathf.Sin(currentAngle);
            
            vertices[i + 1] = new Vector3(x * Radius, y * Radius, 0);            
        }
        


        int[] triangles = new int[Segments * 3];

        for (int i = 0; i < Segments - 1; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2 % Segments;
        }

        triangles[(Segments - 1) * 3] = 0;
        triangles[(Segments - 1) * 3 + 1] = vertices.Length - 1;
        triangles[(Segments - 1) * 3 + 2] = 1;
        
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
