using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExtensions
{
    public static Vector3 xy(this Vector3 v) => new Vector2(v.x, v.y);
    public static Vector3 _yz(this Vector3 v) => new Vector3(0, v.y, v.z);
    public static Vector3 x_z(this Vector3 v) => new Vector3(v.x, 0, v.z);
    public static Vector3 xy_(this Vector3 v) => new Vector3(v.x, v.y, 0);
}
