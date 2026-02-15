using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BezierPoint : MonoBehaviour
{
    public GameObject ControlStart;
    public GameObject ControlEnd;


    public Vector3 GetAnchorPoint()
    {
        return transform.position;
    }

    public Vector3 GetControlStart()
    {
        return ControlStart.transform.position;
    }

    public Vector3 GetControlEnd()
    {
        return ControlEnd.transform.position;
    }

    private void OnDrawGizmos()
    {
        Vector3 aPoint = transform.position;
        Vector3 sPoint = ControlStart.transform.position;
        Vector3 ePoint = ControlEnd.transform.position;

        // Force the EndPoint to be the mirror of the StartPoint
        ControlEnd.transform.position = aPoint - (sPoint - aPoint);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(aPoint, 0.05f);

        Gizmos.color = Color.white;
        Gizmos.DrawSphere(sPoint, 0.05f);
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(ePoint, 0.05f);

        Handles.color = Color.white;
        Handles.DrawLine(aPoint, sPoint);
        Handles.DrawLine(aPoint, ePoint);


    }

}
