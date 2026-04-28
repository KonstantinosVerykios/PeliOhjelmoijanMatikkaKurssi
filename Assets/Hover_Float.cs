using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WaterMineScript : MonoBehaviour
{
    // Detection range and Speed
    [Range(1f, 20f)]
    public float Radius;
    [Range(1f, 5f)]
    public float ChaseSpeed;

    // Rotation and Hovering
    [Range(1f, 100f)]
    public float RotationSpeed;
    [Range(1f, 4f)]
    public float HoverAmplitude;

    //Scaling settings
    public float ScaleAmount = 0.1f;
    public Vector3 BaseScale;
    public GameObject TrackedTarget;

    private float Y;
    private void Start()
    {
        Y = transform.position.y;
        BaseScale = transform.localScale;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 distanceToPlayer = TrackedTarget.transform.position - transform.position;
       if (Radius > distanceToPlayer.magnitude)
       {
            FollowPlayer();
            FloatHover();
            ScaleMine();
        }
    }

    private void ScaleMine()
    {
        float targetScaleMultiplier = 3f;
        transform.localScale = Vector3.Lerp(transform.localScale,BaseScale * targetScaleMultiplier,Time.deltaTime);
    }
    private void FloatHover()
    {
        // Vertical bobbing using sine wave
        float hoverOffset = Mathf.Sin(Time.time * 2f) * HoverAmplitude;

        transform.position = new Vector3(transform.position.x, Y + hoverOffset,transform.position.z);

        // Optional: rotate for that "mine" feel
        transform.Rotate(new Vector3(1f, 2f, 3f), RotationSpeed * Time.deltaTime);
    }

    private void FollowPlayer()
    {
        if (TrackedTarget != null) 
        {
            Debug.Log("Following Player");
        }
        // Move toward the player
        transform.position = Vector3.MoveTowards(transform.position,TrackedTarget.transform.position,ChaseSpeed * Time.deltaTime);

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}
