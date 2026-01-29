using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Scaling : MonoBehaviour
{

    public int ScreenWidth = 1920;
    public int ScreenHeight = 1080;

    public Vector3Int ScreenPosition = new Vector3Int(0, 0, 0);

    [Header("PopUp")]
    public int Percentage = 20;

    [Header("HealthBar")]
    public int HealthBarWidth = 20;
    public int HealthBarHeight = 20;

    public int HealthBarOffsetX = 20;
    public int HealthBarOffsetY = 20;

    [Header("Debug")]
    public bool Debug = true;

    private void OnDrawGizmos()
    {
        Vector3 screenPos = ScreenPosition;
        Drawing.DrawVector(ScreenPosition, Vector3.zero, Color.red, 3.0f, Debug);

        Drawing.DrawSquare(screenPos, ScreenHeight, ScreenWidth, Color.white, 5.0f);

    }

}
