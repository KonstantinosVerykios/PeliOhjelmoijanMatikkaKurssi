using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]

public class PlaneGenerator : MonoBehaviour
{
    private Mesh mesh;

    public bool DEBUG = false;

    [Range(10f, 200f)]
    public float Size = 10f;

    [Range(10, 100)]
    public int Segments = 10;

    [Header("Perlin Noise Modifiers")]
    public List<PerlinNoise> NoiseModifiers = new List<PerlinNoise>();

    private List<Vector3> Vertices = new List<Vector3>();
    private List<int> Triangles = new List<int>();
    private List<Vector3> UV = new List<Vector3>();

    [System.Serializable]
    public class PerlinNoise
    {
        [Range(0f, 5f)]
        public float Density;
        [Range(0f, 5f)]
        public float Amplitude;
    }

    private void GeneratePlane()
    {
        if(mesh == null)
        {
            mesh = new Mesh();
            mesh.name = "TerrainPlane";
        }
        else
        {
            mesh.Clear();
        }
        
        //Clear all
        Vertices.Clear();
        Triangles.Clear();
        UV.Clear();

        

        //Create and add the vertices to the vertices list
        float delta = Size / Segments;

        for (int i = 0; i <= Segments; i++)
        {
            for (int j = 0; j <= Segments; j++)
            {
                float x = i * delta;
                float z = j * delta;

                float height = 0f;

                // Handle all the created perlin noises
                foreach (PerlinNoise noise in NoiseModifiers)
                {
                    height += noise.Amplitude * Mathf.PerlinNoise(noise.Density * x, noise.Density * z);
                }
                // OLD CODE: Vertices.Add(new Vector3(x, totalAmplitude * Mathf.PerlinNoise(totalDensity * x, totalDensity * z), z));
                Vertices.Add(new Vector3(x, height, z));

                UV.Add(new Vector3(x / Size, z / Size));
            }
        }
        

        //For each segment we want to add 2 triangles into the Triangles list
        for(int i = 0; i < Segments; i++)
        {   // i = current segment
            for (int j = 0; j < Segments; j++)
            {
                
                int cur_first = i * (Segments + 1) + j;
                int cur_second = cur_first + 1;

                int next_first = cur_first + Segments + 1;
                int next_second = next_first + 1;

                Triangles.Add(cur_first);
                Triangles.Add(cur_second);
                Triangles.Add(next_first);

                Triangles.Add(cur_second);
                Triangles.Add(next_second);
                Triangles.Add(next_first);
            }
        }
        mesh.SetVertices(Vertices);
        mesh.SetTriangles(Triangles, 0);
        mesh.SetUVs(0, UV);
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    private void OnDrawGizmos()
    {
        GeneratePlane();

        if (DEBUG)
        {
            // Check if the planes vertices are in the correct place

            float delta = Size / Segments;
            Debug.Log("Delta: " + delta);

            Gizmos.color = Color.yellow;

            //Loop the x y grid
            for (int i = 0; i <= Segments; i++)
            {
                for (int j = 0; j <= Segments; j++)
                {
                    float x = i * delta;
                    float z = j * delta;

                    Gizmos.DrawSphere(new Vector3(x, 0, z), 0.3f);
                }
            }
        }
    }
}
