using UnityEngine;
using System.Collections;

public class Face
{
    public float d;
    public Vector3 n;

    public Vector3 p0;
    public Vector3 p1;
    public Vector3 p2;

    public void SetUp()
    {
        n = Vector3.Cross(p0 - p1,p2 - p1);
        d = Vector3.Dot(p0, n);
    }
}
