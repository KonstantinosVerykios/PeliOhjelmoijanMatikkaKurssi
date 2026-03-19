using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]

public class DonutMesh : MonoBehaviour
{

    private Mesh mesh;

    [Range(0.1f, 10f)]
    public float InnerRadius = 1.0f;

    [Range(3, 32)]
    public int Segments = 8;

    [Range(0.1f,10f)]
    public float Thickness = 1.0f;

    List<Vector3> vertices = new List<Vector3>();
    List<int> tri_Indices = new List<int>();

    private List<Vector2> UVs = new List<Vector2>();

    private void GenerateDonutMesh()
    {
        //Create a mesh with the vertices: (0,0) (1,0) (0,1) (1,1)
        if (mesh == null)
        {
            mesh = new Mesh();
        }
        else mesh.Clear();

        vertices.Clear();
        tri_Indices.Clear();
        UVs.Clear();

        float deltaAngle = 2.0f * Mathf.PI / Segments;

        for (int i = 0; i < Segments; i++)
        {
            float currentAngle = deltaAngle * i;

            float x = Mathf.Cos(currentAngle);
            float y = Mathf.Sin(currentAngle);

            vertices.Add(new Vector3(x * InnerRadius, y * InnerRadius, 0));
            vertices.Add(new Vector3(x * (InnerRadius + Thickness), y * (InnerRadius + Thickness), 0));

            UVs.Add(new Vector2(0, 0));
            UVs.Add(new Vector2((x+1f)/2, (y+1f)/2));

        }


        for (int i = 0; i < Segments - 1; i++)
        {
            tri_Indices.Add(i * 2);
            tri_Indices.Add(i * 2 + 3);
            tri_Indices.Add(i * 2 + 1);

            tri_Indices.Add(i * 2);
            tri_Indices.Add(i * 2 + 2);
            tri_Indices.Add(i * 2 + 3);
        }

        tri_Indices.Add((Segments - 1) * 2);
        tri_Indices.Add(1);
        tri_Indices.Add((Segments - 1) * 2 + 1);
        
        tri_Indices.Add((Segments - 1) * 2);
        tri_Indices.Add(0);
        tri_Indices.Add(1);

        mesh.SetVertices(vertices);
        mesh.SetTriangles(tri_Indices, 0);
        mesh.RecalculateNormals();
        mesh.SetUVs(0, UVs);

        
        GetComponent<MeshFilter>().sharedMesh = mesh;

    }


    private void OnDrawGizmos()
    {
       
        GenerateDonutMesh();

        for (int i = 0; i < vertices.Count; i++)
        {
            Gizmos.DrawSphere(vertices[i] + transform.position, 0.1f);
        }
    }
}
