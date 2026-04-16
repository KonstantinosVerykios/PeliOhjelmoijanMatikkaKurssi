using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class PoissonDiscSamplingMulti : MonoBehaviour
{

    [Serializable]
    public class PoissonDiscParams
    {
        public float Radius = 1f;
        public int MaxTries = 30;
        public Color DrawColor = Color.blue;
        public GameObject ObjectToSpawn;
    }

    public float Width = 50f;
    public float Height = 50f;


    public PoissonDiscParams[] PoissonParams;

    public List<GameObject> SpawnedObjects;
    //[Range(0.1f, 10.0f)]
    //public float Radius = 1.0f;
    //[Range(0.1f, 10.0f)]
    //public float BigRadius = 2.0f;
    //[Range(3, 50)]
    //public int MaxTries = 30;

    public bool Generate = false;

    private List<List<Vector2>> points_lists = new List<List<Vector2>>();

    //private List<Vector2> points = new List<Vector2>();

    //private List<Vector2> bigger_points = new List<Vector2>();

    bool isValid(List<Vector2> points, Vector2 point, float radius, float width, float height)
    {
        if (point.x < 0 || point.x > width || point.y < 0 || point.y > height)
            return false;

        for (int i = 0; i < points.Count; i++)
        {
            if ((points[i] - point).magnitude < radius)
                return false;
        }

        return true;
    }

    private List<Vector2> PoissonDiscSample(float w, float h, float r, int k)
    {
        List<Vector2> samples = new List<Vector2>();
        List<Vector2> spawnPoints = new List<Vector2>();

        // Add the first spawn point to the middle of the area
        spawnPoints.Add(new Vector2(w / 2f, h / 2f));

        while (spawnPoints.Count > 0)
        {
            // Try to generate new points around one of the spawnpoints in the list
            int randIndex = Random.Range(0, spawnPoints.Count);

            // This is our spawn center
            Vector2 spawnCenter = spawnPoints[randIndex];

            bool Accepted = false;

            // (maximum k tries)
            for (int i = 0; i < k; i++)
            {
                // Generate new point
                float rndAngle = Random.Range(0f, 2f * Mathf.PI);
                Vector2 rndPosition = new Vector2(Mathf.Cos(rndAngle), Mathf.Sin(rndAngle));
                rndPosition *= Random.Range(r, 2 * r);
                rndPosition += spawnCenter;

                // Check if the point is valid
                if (isValid(samples, rndPosition, r, w, h))
                {
                    samples.Add(rndPosition);
                    spawnPoints.Add(rndPosition);
                    Accepted = true;
                    break;
                }
            }

            if (!Accepted)
            {
                // Could not generate a new point, so remove this spawnpoint
                spawnPoints.RemoveAt(randIndex);
            }

        }

        return samples;
    }

    private Transform spawnParent;

    private void SpawnObjects()
    {

        if (spawnParent == null)
        {
            GameObject parentObj = new GameObject("SpawnedObjects");
            spawnParent = parentObj.transform;
        }

        for (int i = spawnParent.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(spawnParent.GetChild(i).gameObject);
        }

        for (int i = 0; i < points_lists.Count; i++)
        {
            for (int j = 0; j < points_lists[i].Count; j++)
            {
                Vector2 point = points_lists[i][j];
                RaycastHit hit;

                Vector3 origin = new Vector3(point.x, 0, point.y);

                

                if (Physics.Raycast(origin, Vector3.down, out hit, Mathf.Infinity))
                {
                    
                    GameObject obje = Instantiate(PoissonParams[i].ObjectToSpawn, hit.point, Quaternion.identity, spawnParent);
                }
            }
        }
    }


    private void OnDrawGizmos()
    {
        if (Generate)
        {
            

            points_lists.Clear();
            for (int i = 0; i < PoissonParams.Length; i++)
            {
                List<Vector2> points = new List<Vector2>();
                points = PoissonDiscSample(Width, Height, PoissonParams[i].Radius, PoissonParams[i].MaxTries);
                points_lists.Add(points);
            }

            

            // Remove points that overlap within radii???
            List<Vector2> testpoints;
            List<Vector2> otherpoints;
            for (int i = points_lists.Count - 2; i >= 0; i--)
            {
                testpoints = points_lists[i];
                List<int> toRemove = new List<int>();

                for (int j = i + 1; j < points_lists.Count; j++)
                {
                    otherpoints = points_lists[j];
                    //otherpoints = points_lists[i+1];
                    for (int m = 0; m < testpoints.Count; m++)
                    {
                        for (int n = 0; n < otherpoints.Count; n++)
                        {
                            if ((testpoints[m] - otherpoints[n]).magnitude < PoissonParams[i].Radius)
                            {
                                // Mark it for deletion
                                if (!toRemove.Contains(m))
                                    toRemove.Add(m);
                            }
                        }
                    }
                }

                toRemove.Sort();
                toRemove.Reverse();
                foreach (var index in toRemove)
                {
                    testpoints.RemoveAt(index);
                }
            }

            SpawnObjects();
            Generate = false;
        }

        // ... Draw the points ...
        for (int i = 0; i < points_lists.Count; i++)
        {
            Gizmos.color = PoissonParams[i].DrawColor;
            List<Vector2> points = points_lists[i];
            for (int j = 0; j < points.Count; j++)
            {
                //Handles.DrawWireDisc(new Vector3(points[j].x, 0, points[j].y), Vector3.up, 0.5f * PoissonParams[i].Radius);
                //Gizmos.DrawSphere(new Vector3(points[j].x, 0, points[j].y), PoissonParams[i].Radius * 0.1f);
            }
        }
    }
}
