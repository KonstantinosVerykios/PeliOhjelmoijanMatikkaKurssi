using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.UI.Image;

public class ObjectPlacement : MonoBehaviour
{
    public bool active = true;
    private void OnDrawGizmos()
    {
        RaycastHit hit; 

        if (Physics.Raycast( transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            Handles.color = Color.black;
            Handles.DrawLine(hit.point, transform.position);
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(hit.point, 0.01f);

            Drawing.DrawVector(hit.normal, hit.point, Color.green, 3f);

            //Drawing.DrawVector(transform.forward, hit.point, Color.blue, 3f);

            Vector3 right = Vector3.Cross(hit.normal, transform.forward);
            Drawing.DrawVector(right, hit.point, Color.red, 3f);

            Vector3 forward = Vector3.Cross(right, hit.normal);
            Drawing.DrawVector(forward, hit.point, Color.blue, 3f);
        }
        else
        {
            Handles.color = Color.black;
            Handles.DrawLine(transform.position, hit.point, 10f);
        }
    }
}
