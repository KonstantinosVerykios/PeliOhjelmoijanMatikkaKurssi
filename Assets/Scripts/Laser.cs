using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float VectorScale = 5f;
    public bool Debug = true;

    [Range(1, 100)]
    public int Bounces = 10;
    private void OnDrawGizmos()
    {

        RaycastHit hit;

        Vector3 origin = transform.position;
        Vector3 direction = transform.right;

        for (int i = 0; i < Bounces; i++)
        {
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(origin, direction, out hit, Mathf.Infinity))
            {
            
                Handles.color = Color.red;
                Handles.DrawLine(origin, hit.point, 5f);
                if (Debug)
                {
                    // Raycastatun pinnan normaali
                    Drawing.DrawVector(VectorScale * hit.normal, hit.point, Color.green, 3f, Debug);

                    // Raycastatun pinnan negatiivinen normaali
                    Drawing.DrawVector(VectorScale * -hit.normal, hit.point, Color.green, 3f, Debug);

                    Drawing.DrawVector(VectorScale * direction, hit.point, Color.red, 3f, Debug);

                    // Kaava: projektio = (x"dot" v / v"dot" v )*v
                    Vector3 projection = hit.normal * Vector3.Dot(direction, hit.normal / Vector3.Dot(hit.normal, hit.normal));

                    // 
                    Drawing.DrawVector(VectorScale * projection, hit.point, Color.magenta, 3f, Debug);

                    //
                    Drawing.DrawVector(VectorScale * -projection, hit.point + VectorScale * direction, Color.magenta, 3f, Debug);
                    Drawing.DrawVector(VectorScale * -projection, hit.point + VectorScale * (direction - projection), Color.magenta, 3f, Debug);

                    Vector3 reflect = (direction - 2 * projection);

                    Drawing.DrawVector(VectorScale * reflect, hit.point, Color.red, 3f, Debug);
                }
            

                //Use Vector3.Reflect()
                Vector3 reflectUnity = Vector3.Reflect(direction, hit.normal);
                if (Debug)
                {
                    Drawing.DrawVector(VectorScale * reflectUnity, hit.point, Color.black, 3f, Debug);
                }
                direction = reflectUnity;
            }
            else
            {
                Handles.color = Color.black;
                Handles.DrawLine(transform.position, hit.point, 10f);
            }

            origin = hit.point;
        }
    }
}
