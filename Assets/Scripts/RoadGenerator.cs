using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class RoadGenerator : MonoBehaviour
{

    List<BezierPoint> CurvePoints = new List<BezierPoint>();

    [SerializeField]
    Mesh2D RoadMesh;

    public BezierPoint[] Points;

    public bool ClosePath;

    [Range(10f, 100f)]
    [SerializeField] int SegmentAmount;

    private float segmentsT;

    private int CurSegment;
    
    List<Vector3> vertices = new List<Vector3>();

    private Mesh mesh;

    private void GenerateMesh()
    {
        if (mesh == null)
        {
            mesh = new Mesh();
            mesh.name = "RoadMesh";
        }
        else
            mesh.Clear();

        List<Vector3> meshVertices = new List<Vector3>();
        List<int> meshIndices = new List<int>();

        int bezierSegments = CurvePoints.Count - 1;

        for (int segment = 0; segment < SegmentAmount; segment++)
        {
            float T = (float)segment / (SegmentAmount - 1);
            float scaledT = T * bezierSegments;

            int curSegment = Mathf.Min(Mathf.FloorToInt(scaledT), bezierSegments - 1);
            float t = scaledT - curSegment;

            Vector3 point = CalculateBezier(
                CurvePoints[curSegment].GetAnchorPoint(),
                CurvePoints[curSegment].GetControlEnd(),
                CurvePoints[curSegment + 1].GetControlStart(),
                CurvePoints[curSegment + 1].GetAnchorPoint(),
                t);

            Vector3 forward = CalculateBezierDirection(
                CurvePoints[curSegment].GetAnchorPoint(),
                CurvePoints[curSegment].GetControlEnd(),
                CurvePoints[curSegment + 1].GetControlStart(),
                CurvePoints[curSegment + 1].GetAnchorPoint(),
                t);

            Vector3 right = Vector3.Cross(Vector3.up, forward).normalized;

            for (int i = 0; i < RoadMesh.vertices.Length; i++)
            {
                Vector3 localPoint = point 
                    + right * RoadMesh.vertices[i].point.x 
                    + Vector3.up * RoadMesh.vertices[i].point.y;

                localPoint = transform.InverseTransformPoint(localPoint);
                meshVertices.Add(localPoint);
            }
        }

        for (int segment = 0; segment < SegmentAmount - 1; segment++)
        {
            int currentVert = segment * RoadMesh.vertices.Length;
            int nextVert = (segment + 1) * RoadMesh.vertices.Length;

            for (int i = 0; i < RoadMesh.vertices.Length - 1; i++)
            {
                meshIndices.Add(currentVert + i);
                meshIndices.Add(nextVert + i);
                meshIndices.Add(nextVert + i + 1);

                meshIndices.Add(currentVert + i);
                meshIndices.Add(nextVert + i + 1);
                meshIndices.Add(currentVert + i + 1);
            }
        }

        mesh.SetVertices(meshVertices);
        mesh.SetTriangles(meshIndices, 0);
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().sharedMesh = mesh;
    }
    private void OnDrawGizmos()
    {

        vertices.Clear();
        GenerateMesh();

        CurvePoints = Points.ToList();

        if (ClosePath)
            CurvePoints.Add(CurvePoints[0]);
        
        int bezierSegments = CurvePoints.Count - 1;

        //For all the segments => Loop through
        for (int segment = 0; segment < SegmentAmount; segment++)
        {
            float T = (float)segment / (SegmentAmount - 1);
            float scaledT = T * bezierSegments;

            CurSegment = Mathf.Min(Mathf.FloorToInt(scaledT), bezierSegments - 1);
            segmentsT = scaledT - CurSegment;

            Gizmos.color = Color.blue;

            Vector3 point = CalculateBezier(CurvePoints[CurSegment].GetAnchorPoint(), CurvePoints[CurSegment].GetControlEnd(), CurvePoints[CurSegment + 1].GetControlStart(), CurvePoints[CurSegment + 1].GetAnchorPoint(), segmentsT);

            Vector3 forward = CalculateBezierDirection(CurvePoints[CurSegment].GetAnchorPoint(), CurvePoints[CurSegment].GetControlEnd(), CurvePoints[CurSegment + 1].GetControlStart(), CurvePoints[CurSegment + 1].GetAnchorPoint(), segmentsT);

            Vector3 right = Vector3.Cross(Vector3.up, forward).normalized;

            GenerateRoad(segment, point, right);
        }
    }

    void GenerateRoad(int seg, Vector3 point, Vector3 right)
    {
        //Road Spheres on Vertices
        for (int i = 0; i < RoadMesh.vertices.Length; i++)
        {
            Gizmos.DrawSphere(point + right * RoadMesh.vertices[i].point.x + Vector3.up * RoadMesh.vertices[i].point.y, 0.2f);
        }

        //RoadVertices
        for (int i = 0; i < RoadMesh.vertices.Length - 1; i += 2)
        {
            vertices.Add(point + right * RoadMesh.vertices[i].point.x + Vector3.up * RoadMesh.vertices[i].point.y);

            Vector3 p1 = point + right * RoadMesh.vertices[i].point.x + Vector3.up * RoadMesh.vertices[i].point.y;

            Vector3 p2 = point + right * RoadMesh.vertices[(i >= RoadMesh.vertices.Length - 2) ? 0 : i + 2].point.x + Vector3.up * RoadMesh.vertices[(i >= RoadMesh.vertices.Length - 2) ? 0 : i + 2].point.y;

            Handles.DrawLine(p1, p2);
        }

        if (seg == 0)
            return;

        // RoadLines
        for (int i = 0; i < Mathf.FloorToInt(RoadMesh.vertices.Length * 0.5f); i++)
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
