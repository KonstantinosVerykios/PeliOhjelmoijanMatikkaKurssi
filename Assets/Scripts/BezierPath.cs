using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BezierPath : MonoBehaviour
{

    public BezierPoint[] Points;

    [Range (0f, 1f), Tooltip("T-value for the whole path (global)")]
    public float GlobalT = 0.0f;

    public bool ClosePath;
    
    private void OnDrawGizmos()
    {
        int segments = Points.Length - 1;

        for (int i = 0; i < segments; i++)
        {
            Handles.DrawBezier(
                Points[i].GetAnchorPoint(), 
                Points[i + 1].GetAnchorPoint(), 
                Points[i].GetControlEnd(), 
                Points[i+1].GetControlStart(), Color.magenta, null, 3f);

            if (ClosePath)
            {
                Handles.DrawBezier(Points[segments].GetAnchorPoint(), 
                    Points[0].GetAnchorPoint(), 
                    Points[segments].GetControlEnd(), 
                    Points[0].GetControlStart(), Color.magenta, null, 3f);
            }
        }

        int startIndex = (int)(GlobalT * segments);
        Debug.Log("startIndex: " + startIndex);

        if(startIndex == segments)
        {
            startIndex = segments - 1;
        }
    }
}
