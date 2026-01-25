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

    public int HealthOffsetX = 20;
    public int HealthOffsetY = 20;

    [Header("Debug")]
    public bool Debug = true;

    private void OnDrawGizmos()
    {
        
        //Change ScreenPosition Vector3Int to Vector3
        Vector3 screenPos = ScreenPosition;

        // Draw the first vector. The screenPos is taken from the inspectors values
        Drawing.DrawVector(screenPos, Vector3.zero, Color.red, 3.0f, Debug);

        // Draw a square from the users inputted location
        Drawing.DrawSquare(screenPos, ScreenHeight, ScreenWidth, Color.white, 5.0f);

        // Draw a vector that points into the middle of the first white square
        Drawing.DrawVector(Vector3.up * ScreenHeight * 0.5f + Vector3.right * ScreenWidth * 0.5f, screenPos, Color.blue, 3.0f, Debug);

        // Create a seperate vector that points to the center
        Vector3 firstSquareCenter = screenPos + Vector3.right * (ScreenWidth * 0.5f)+ Vector3.up * (ScreenHeight * 0.5f);

        // PopUps width and height based on the inputed percentage
        int popupWidth = ScreenWidth * Percentage / 100;
        int popupHeight = ScreenHeight * Percentage / 100;

        // The Drawing.DrawSquare function creates asquare form the bottom left corner of the square so we need to figure out its position
        Vector3 popupBottomLeft = firstSquareCenter - Vector3.right * (popupWidth * 0.5f) - Vector3.up * (popupHeight * 0.5f);

        //Once we have the popUps staring position, width and height we can draw it
        Drawing.DrawSquare(popupBottomLeft, popupHeight ,popupWidth , Color.white, 5.0f);

        Vector3 healthBarStart = screenPos + Vector3.right * ScreenWidth + Vector3.up * ScreenHeight;
        Vector3 healthBarOffset = Vector3.down * HealthOffsetX + Vector3.left * HealthOffsetY;
        Drawing.DrawVector(healthBarOffset, healthBarStart, Color.magenta, 3.0f, Debug);

        Drawing.DrawSquare(healthBarStart + healthBarOffset, -HealthBarHeight, -HealthBarWidth, Color.white, 3.0f);



    }

}
