using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static Vector2 Mid2D(this Transform t)
    {
        return new Vector2(t.position.x, t.position.z);
    }

    public static Vector3 Mid3D(this Transform t)
    {
        return t.position;
    }
}
