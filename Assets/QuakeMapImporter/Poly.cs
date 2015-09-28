using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Poly
{
    public List<Vector3> verts = new List<Vector3>();
    public int[] tris = {0,1,2,1,2,3};

    public Vector3 normal;

    public void AddVertex(Vector3 v, Vector3 n)
    {
        if (!verts.Contains(v))
        {
            verts.Add(v);
            normal = n.normalized;
        }
    }

    public void Order()
    {
        if (normal == Vector3.up)
        {
            tris = new int[] { 2, 1, 0, 1, 2, 3};
        }
        else if(normal == Vector3.down)
        {
            tris = new int[] { 0, 1, 2, 3, 2, 1 };
        }
        else if(normal == Vector3.right)
        {
            tris = new int[] { 0, 1, 2, 3, 2, 1 };
        }
        else if (normal == Vector3.left)
        {
            tris = new int[] { 2, 1, 0, 1, 2, 3 };
        }
        else if(normal == Vector3.forward)
        {
            tris = new int[] { 2, 1, 0, 1, 2, 3 };
        }
        else if (normal == Vector3.back)
        {
            tris = new int[] { 0, 1, 2, 3, 2, 1 };
        }

    }
}
