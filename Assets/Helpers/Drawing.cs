using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Drawing : MonoBehaviour
{
    public static void DrawVector(Vector3 vec, Vector3 pos, Color originalColor, float thickness = 5.0f, bool active = true)
    {
        if (!active)
        {
            Handles.color = Color.clear;
        }
        else Handles.color = originalColor;

        //Store original color
        Color color = Handles.color;

        //Change it
        Handles.color = originalColor;

        // Draw the line from pos to pos+vec
        Handles.DrawLine(pos, pos + vec, thickness);

        //Draw the arrow head
        Vector3 n = vec.normalized;
        Handles.ConeHandleCap(0, vec + pos - 0.35f * n, Quaternion.LookRotation(vec), 0.1f, EventType.Repaint);

    }

    public static void DrawSquare(Vector3 pos, int height, int width, Color color, float thickness=0.0f)
    {
        Handles.color = Color.white;
        Handles.DrawLine(pos, pos + Vector3.up * height, 2f);
        Handles.DrawLine(pos, pos + Vector3.right * width, 2f);
        Handles.DrawLine(pos + Vector3.right * width, pos + Vector3.up * height + Vector3.right * width, 2f);
        Handles.DrawLine(pos + Vector3.up * height, pos + Vector3.right * width + Vector3.up * height, 2f);
    }
}
