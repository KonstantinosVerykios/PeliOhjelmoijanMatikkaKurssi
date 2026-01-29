using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Interpolate : MonoBehaviour
{
    public GameObject cube;

    public GameObject A;
    public GameObject B;

    [Range(0, 2f)]
    public float t = 0f;

    
    [Header("Interpolation")]
    public float AtoB = 0f;
    public EasingFunction.Ease EaseType = EasingFunction.Ease.EaseInOutQuad;
    private EasingFunction.Function EasingFunc;

    private void OnDrawGizmos()
    {
        Vector3 a = A.transform.position;
        Vector3 b = B.transform.position;

        Drawing.DrawVector(a, Vector3.zero, Color.red, 5f, true);
        Drawing.DrawVector(b, Vector3.zero, Color.green, 5f, true);

        Handles.DrawDottedLine(a, b, 1f);

        Vector3 partA = a * (1 - t);
        Vector3 partB = b * t;

        Drawing.DrawVector(partA, Vector3.zero, Color.green, 5f, true);
        Drawing.DrawVector( partB, Vector3.zero, Color.red, 5f, true);

        Drawing.DrawVector(partB + partA, Vector3.zero, Color.black, 5f, true);

    }

    Vector3 Lerp(Vector3 a, Vector3 b, float t)
    {
        return a * (1 - t) + b * t;
    }

    private void Start()
    {
        //Get selected function
        EasingFunc = EasingFunction.GetEasingFunction(EaseType);
    }
    private void Update()
    {
        float t = Time.time / AtoB;
        if (t > 1f) t = 1f;

        if (cube != null)
        {
            cube.transform.position = Lerp(A.transform.position, B.transform.position, EasingFunc(0f, 1f, t));
        }   
    }
}
