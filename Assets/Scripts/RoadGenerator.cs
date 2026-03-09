using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{

    List<BezierPoint> CurvePoints = new List<BezierPoint>();

    [SerializeField]
    Mesh2D RoadMesh;

    public BezierPoint[] Points;

    public bool ClosePath;

    [Range(10f, 100f)]
    [SerializeField] int Segments;

    private float segmentsT;

    private int CurSegment;
    
    List<Vector3> vertices = new List<Vector3>();

    private void OnDrawGizmos()
    {
        vertices.Clear();

        CurvePoints = Points.ToList();

        if (ClosePath)
            CurvePoints.Add(CurvePoints[0]);
        

        int bezierSegments = CurvePoints.Count - 1;

        for (int segment = 0; segment < Segments; segment++)
        {
            float T = (float)segment / (Segments - 1);
            float scaledT = T * bezierSegments;

            CurSegment = Mathf.Min(Mathf.FloorToInt(scaledT), bezierSegments - 1);

            segmentsT = scaledT - CurSegment;

            Gizmos.color = Color.blue;

            Vector3 point = CalculateBezier(CurvePoints[CurSegment].GetAnchorPoint(), CurvePoints[CurSegment].GetControlEnd(), CurvePoints[CurSegment + 1].GetControlStart(), CurvePoints[CurSegment + 1].GetAnchorPoint(), segmentsT);

            Vector3 forward = CalculateBezierDirection(CurvePoints[CurSegment].GetAnchorPoint(), CurvePoints[CurSegment].GetControlEnd(), CurvePoints[CurSegment + 1].GetControlStart(), CurvePoints[CurSegment + 1].GetAnchorPoint(), segmentsT);

            Vector3 right = Vector3.Cross(Vector3.up, forward);

            GenerateRoad(segment, point, right);
        }
    }

    void GenerateRoad(int seg, Vector3 point, Vector3 right)
    {
        for(int i = 0; i < RoadMesh.vertices.Length; i++)
        {
            Gizmos.DrawSphere(point + right * RoadMesh.vertices[i].point.x + Vector3.up * RoadMesh.vertices[i].point.y, 0.2f);
        }

        for (int i = 0; i < RoadMesh.vertices.Length - 1; i += 2)
        {
            vertices.Add(point + right * RoadMesh.vertices[i].point.x + Vector3.up * RoadMesh.vertices[i].point.y);

            Vector3 p1 = point + right * RoadMesh.vertices[i].point.x + Vector3.up * RoadMesh.vertices[i].point.y;

            Vector3 p2 = point + right * RoadMesh.vertices[(i >= RoadMesh.vertices.Length - 2) ? 0 : i + 2].point.x + Vector3.up * RoadMesh.vertices[(i >= RoadMesh.vertices.Length - 2) ? 0 : i + 2].point.y;
        
            Handles.DrawLine(p1, p2);
        }

        if (seg == 0)
            return;
        
        for(int i = 0; i < Mathf.FloorToInt(RoadMesh.vertices.Length * 0.5f); i++)
        {
            Vector3 p1 = vertices[(seg - 1) * Mathf.FloorToInt(RoadMesh.vertices.Length * 0.5f) + i];

            Vector3 p2 = vertices[seg * Mathf.FloorToInt(RoadMesh.vertices.Length * 0.5f) + i];
            
            Handles.DrawLine(p1, p2);
        }
    }

    private Vector3 CalculateBezierDirection(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        Vector3 x = Vector3.Lerp(p0, p1, t);
        Vector3 y = Vector3.Lerp(p1, p2, t);
        Vector3 z = Vector3.Lerp(p2, p3, t);

        Vector3 r = Vector3.Lerp(x, y, t);
        Vector3 s = Vector3.Lerp(y, z, t);

        return (s-r).normalized;
    }

    private Vector3 CalculateBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        Vector3 x = Vector3.Lerp(p0, p1, t);
        Vector3 y = Vector3.Lerp(p1, p2, t);
        Vector3 z = Vector3.Lerp(p2, p3, t);

        Vector3 r = Vector3.Lerp(x, y, t);
        Vector3 s = Vector3.Lerp(y, z, t);

        return Vector3.Lerp(r, s, t);
    }
}
