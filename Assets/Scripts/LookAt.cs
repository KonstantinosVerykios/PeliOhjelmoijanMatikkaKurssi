using UnityEditor;
using UnityEngine;

public class LookAtTrigger : MonoBehaviour
{
    public GameObject[] Targets;
    Vector3 to_target;

    public GameObject LookAtTarget;
    Vector3 look_at;
    Vector3 look_at_n;

    [Range(0f, 25f)] public float Radius = 10f;
    [Range(-1f, 1f)] public float Threshold = 0.8f;
    [Range(0f, 10f)] public float Height = 1f;

    private void OnDrawGizmos()
    {
        bool in_range = InRange();
        Vector3 user = transform.position;

        float degrees = Mathf.Rad2Deg * Mathf.Acos(Threshold);

        Quaternion rotator = Quaternion.AngleAxis(degrees, Vector3.up);
        Drawing.DrawVector(rotator * look_at.normalized * Radius, user, Color.red, 1f);
        Drawing.DrawVector(Quaternion.Inverse(rotator) * look_at.normalized * Radius, user, Color.red, 1f);

        Color mycolor;

        if (in_range)
        {
            Handles.color = Color.red;
            mycolor = Color.red;
        }
        else
        {
            Handles.color = Color.green;
            mycolor = Color.green;
        }
    
        foreach (var target in Targets)
        {
            if (target == null) continue;
            Vector3 dir = target.transform.position - user;
            Drawing.DrawVector(dir, user, mycolor, 1f);
        }

        Drawing.DrawVector(look_at, user, Color.magenta, 1f);
    }

    public bool InRange()
    {
        look_at = LookAtTarget.transform.position - transform.position;
        look_at_n = look_at.normalized;
        look_at.y = 0f;

        foreach (var target in Targets)
        {
            if (target == null) continue;

            to_target = target.transform.position - transform.position;

            bool inCone =
                Vector3.Dot(to_target.normalized, look_at.normalized) > Threshold;

            bool inDistance =
                look_at_n.magnitude * Radius > to_target.magnitude;

            bool inHeight =
                target.transform.position.y < transform.position.y + Height / 2 &&
                target.transform.position.y > transform.position.y - Height / 2;

            if (inCone && inDistance && inHeight)
                return true;
        }

        return false;
    }
}