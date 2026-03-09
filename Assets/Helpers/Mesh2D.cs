using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "mesh2D", menuName = "Scriptable Objects/Mesh2D")]
public class Mesh2D : ScriptableObject
{
    [Serializable]
    public class Vertex2D
    {
        public Vector2 point;
        public Vector2 normal;
    }

    public Vertex2D[] vertices;
}
