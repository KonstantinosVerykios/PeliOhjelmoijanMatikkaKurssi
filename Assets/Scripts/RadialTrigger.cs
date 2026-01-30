using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Triggers : MonoBehaviour
{
    [Range(4f, 100f)]
    public float Radius = 1f;

    // If the range is -1 the enemy can see behind him
    [Range(-1f, 1f)]
    public float EnemyFOV = 0.7f;

    [Range(0f, 10f)]
    public float ConeHeight = 0.7f;

    private float LookingAngle;
    

    public GameObject Target;
    public GameObject LookAt;


    private Vector3 to_target;
    private Vector3 to_lookAt;

    

    private void OnDrawGizmos()
    {
        SetYZero(LookAt);


        bool in_range = InRange();
        bool looking = LookingAt();
        if (in_range && looking)
        {
            Handles.color = Color.red;
        }
        else if (in_range)
        {
            Handles.color = Color.yellow;
        }
        else
        {
            Handles.color = Color.white;
        }

        //XZ-plane, wirediscs normal is Y-up
        Handles.DrawWireDisc(transform.position, Vector3.up, Radius);

        to_lookAt = LookAt.transform.position - transform.position;

        // Draw the vector from NPC to Player
        Drawing.DrawVector(to_target, transform.position, Color.red , 3f, true);

        // Check if the Player is in the FOV of the NPC if he is turn it black
        if (LookingAt())
        {
            Drawing.DrawVector(to_lookAt, transform.position, Color.black, 3f, true);
        }
        else
        {
            Drawing.DrawVector(to_lookAt, transform.position, Color.white, 3f, true);
        }

        // The angle we can get from Mathf.arcos // Mathf.Acos(EnemyFOV); = Radians
        LookingAngle = Mathf.Rad2Deg*Mathf.Acos(EnemyFOV);
        Quaternion rotator = Quaternion.AngleAxis(LookingAngle, Vector3.up);

        // The right edge of the enemys FOV
        Vector3 look_at_rotated = rotator * to_lookAt;

        
        //Drawing.DrawVector(look_at_rotated, transform.position, Color.blue, 3f, true);

        // The left edge of the enemys FOV by inverting look_at_rotated
        look_at_rotated = Quaternion.Inverse(rotator) * to_lookAt;
        //Drawing.DrawVector(look_at_rotated, transform.position, Color.blue, 3f, true);

        // Creates a wirearc form the left edges point to the center* 2
        //Handles.DrawWireArc(transform.position, Vector3.up, look_at_rotated, LookingAngle * 2, to_lookAt.magnitude);
        

        // WEEKLY ASSIGNMENT

        Vector3 origin = transform.position;
        Vector3 forward = to_lookAt.normalized * look_at_rotated.magnitude;

        Quaternion leftRot = Quaternion.AngleAxis(LookingAngle, Vector3.down);
        Quaternion rightRot = Quaternion.AngleAxis(LookingAngle, Vector3.up);

        Vector3 left = leftRot * forward;
        Vector3 right = rightRot * forward;

        Handles.color = Color.white;

        // Base triangle
        Handles.DrawLine(origin, origin + left);
        Handles.DrawLine(origin, origin + right);
        //Handles.DrawLine(origin + left, origin + right);
        Handles.DrawWireArc(origin, Vector3.up, look_at_rotated, LookingAngle * 2, forward.magnitude);

        // "Support Pillars"
        Vector3 up = Vector3.up * ConeHeight;

        Handles.DrawLine(origin, origin + up);
        Handles.DrawLine(origin + left, origin + left + up);
        Handles.DrawLine(origin + right, origin + right + up);
        

        // Top triangle
        Handles.DrawLine(origin + up, origin + left + up);
        Handles.DrawLine(origin + up, origin + right + up);
        //Handles.DrawLine(origin + left + up, origin + right + up);
        Handles.DrawWireArc(origin + up, Vector3.up, look_at_rotated, LookingAngle * 2, forward.magnitude); ;

    }

    public bool InRange()
    {
        // End point - start point = the vector that leads to it
        to_target = Target.transform.position - transform.position;
        // Make the plane flat so everything is on y = 0

        // If the vector is smaller then the radius the player is inside the radius
        if(to_target.magnitude < Radius)
            return true;
        else
            return false;
    }

    public bool LookingAt()
    {
        Vector3 to_targetN = to_target.normalized;
        Vector3 to_lookAtN = to_lookAt.normalized;

        if(Vector3.Dot(to_lookAtN, to_targetN) > EnemyFOV && to_target.y < ConeHeight && to_target.y >=0)
            return true;
        else 
            return false;

    }

    public void SetYZero(GameObject obj)
    {
        Vector3 pos = obj.transform.position;
        pos.y = 0f;
        obj.transform.position = pos;
    }
}
