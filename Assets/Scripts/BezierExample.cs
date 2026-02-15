using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BezierExample : MonoBehaviour
{
    //Bezier path
    public GameObject A;
    public GameObject B;
    public GameObject C;
    public GameObject D;

    [Range(0f, 1f)]
    public float T = 0.0f;

    private void OnDrawGizmos()
    {
        // Get the position in  a simple variable
        Vector3 a = A.transform.position;
        Vector3 b = B.transform.position;
        Vector3 c = C.transform.position;
        Vector3 d = D.transform.position;

        // Draw the lines between A B C
        Handles.color = Color.blue;
        Handles.DrawLine(a, b);
        Handles.DrawLine(b, c);
        Handles.DrawLine(c, d);

        // Start the drawing the bezier curves lerps
        // Lerp the first three lines
        Vector3 x = (1 - T) * a + T * b;
        Vector3 y = (1 - T) * b + T * c;
        Vector3 z = Vector3.Lerp(c, d, T);

        Gizmos.color = Handles.color;
        Gizmos.DrawSphere(x, 0.05f);
        Gizmos.DrawSphere(y, 0.05f);
        Gizmos.DrawSphere(z, 0.05f);

        // Second level
        Handles.color = Color.white;
        Handles.DrawLine(x, y);
        Handles.DrawLine(y, z);

        
        Vector3 i = Vector3.Lerp(x, y, T);
        Vector3 j = Vector3.Lerp(y, z, T);

        Gizmos.color = Handles.color;
        Gizmos.DrawSphere(i, 0.05f);
        Gizmos.DrawSphere(j, 0.05f);

        //Third level
        Handles.color = Color.red;
        Handles.DrawLine(i, j);

        Vector3 omega = Vector3.Lerp(i, j, T);

        Gizmos.color = Handles.color;
        Gizmos.DrawSphere(omega, 0.05f);


        // Draw the Bezier with unity function
        Handles.DrawBezier(a, d, b, c, Color.yellow, null, 3f);

    }
}
