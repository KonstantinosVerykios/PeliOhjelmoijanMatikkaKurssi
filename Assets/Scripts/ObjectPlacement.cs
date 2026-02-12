using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.UI.Image;

public class ObjectPlacement : MonoBehaviour
{
    public bool active = true;

    public GameObject PlaceableObject;
    public GameObject Target;
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

            Quaternion applyRot = Quaternion.LookRotation(forward, hit.normal);

            //Placeable object rotation and position
            PlaceableObject.transform.localPosition = hit.point;
            PlaceableObject.transform.rotation = applyRot;


            Vector3 toTargetN = (Target.transform.position - PlaceableObject.transform.position).normalized;

            float side = Vector3.Dot(PlaceableObject.transform.right, toTargetN);

            // We now check the dot product
            if (side > 0f)
            {
                //Vector from car to cube RED if on the right, blue if left and Black for 0
                Drawing.DrawVector(toTargetN, PlaceableObject.transform.position, Color.red, 3f);
            }
            else if (side < 0f) 
            {
                Drawing.DrawVector(toTargetN, PlaceableObject.transform.position, Color.blue, 3f);
            }
            else Drawing.DrawVector(toTargetN, PlaceableObject.transform.position, Color.black, 3f);

        }
        else
        {
            Handles.color = Color.black;
            Handles.DrawLine(transform.position, hit.point, 10f);
        }
    }
}
