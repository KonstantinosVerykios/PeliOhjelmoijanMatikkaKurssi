using System.Collections.Generic;
using UnityEngine;

public class PoissonDiscSampling : MonoBehaviour
{

    public float width = 50f;
    public float height = 50f;

    [Range(0.1f, 10.0f)]
    public float radius = 1.0f;

    [Range(0.1f, 10.0f)]
    public float bigRadius = 2.0f;

    [Range(3, 50)]
    public int maxTries = 30;

    private List<Vector2> points = new List<Vector2>();
    private List<Vector2> biggerPoints = new List<Vector2>();

    public bool generate = false;

    private List<Vector2> PoissonDiscSample(float _width, float _height, float _radius, int k)
    {
        List<Vector2> samples = new List<Vector2>();
        List<Vector2> spawnPoints = new List<Vector2>();

        // Add the first spawn point to the middle of the area
        spawnPoints.Add(new Vector2(_width / 2f, _height / 2f));

        while (spawnPoints.Count > 0)
        {
            // Try to generate new points around one of the spawnpoints in the list
            int rndIndex = Random.Range(0, spawnPoints.Count);

            // This is our spawn center
            Vector2 spawnCenter = spawnPoints[rndIndex];

            bool accepted = false;

            // (maximum k tries)
            for (int i = 0; i < k; i++)
            {
                // Generate new point
                float rndAngle = Random.Range(0f, 3f * Mathf.PI);
                Vector2 rndPosition = new Vector2(Mathf.Cos(rndAngle), Mathf.Sin(rndAngle));
                rndPosition *= Random.Range(_radius, 2 * _radius);
                rndPosition += spawnCenter;

                // Check if the point is valid
                if (isValid(samples, rndPosition, _width, _height, _radius))
                {
                    samples.Add(rndPosition);
                    spawnPoints.Add(rndPosition);
                    accepted = true;
                    break;
                }
            }

            if (!accepted)
            {
                // Could not generate a new point, so remove this spawnpoint
                spawnPoints.RemoveAt(rndIndex);
            }
        }

        return samples;
    }

    private bool isValid(List<Vector2> _points, Vector2 _point, float _width, float _height, float _radius)
    {
        if (_point.x < 0 || _point.x > _width || _point.y < 0 || _point.y > _height)
            return false;

        for (int i = 0; i < _points.Count; i++)
        {
            if ((_points[i] - _point).magnitude < _radius)
                return false;
        }

        return true;
    }

    public void OnDrawGizmos()
    {
        if (generate)
        {
            points.Clear();
            points = PoissonDiscSample(width, height, radius, maxTries);

            biggerPoints.Clear();
            biggerPoints = PoissonDiscSample(width, height, bigRadius, maxTries);

            generate = false;
        }

        Gizmos.color = Color.red;

        // Draw the points
        foreach (Vector2 point in points)
        {
            Gizmos.DrawSphere(new Vector3(point.x, 0, point.y), radius * 0.2f);
        }

        Gizmos.color = Color.blue;
        foreach (Vector2 point in biggerPoints)
        {
            Gizmos.DrawSphere(new Vector3(point.x, 0, point.y), bigRadius * 0.2f);
        }

    }
}