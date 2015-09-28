using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Brush : MonoBehaviour
{
    public Face[] faces;
    public Poly[] polys;
    public MeshFilter meshFilter;

    public void SetUp(Transform container, Face[] vectors)
    {
        faces = vectors;
        polys = new Poly[6];

        for (int i = 0; i < polys.Length; i++)
        {
            polys[i] = new Poly();
        }

        foreach (Face f in faces) f.SetUp();

        gameObject.isStatic = true;
        meshFilter = gameObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        transform.SetParent(container);

        GenerateVerts();
        MakeMesh();
        gameObject.AddComponent<BoxCollider>();
        meshRenderer.material = new Material(Shader.Find("Standard"));
    }

    void GenerateVerts()
    {
        bool legal = false;

        for(int i = 0; i < faces.Length - 2; i++)
        {
            for(int j = 0; j < faces.Length - 1; j++)
            {
                for(int k = 0; k < faces.Length; k++)
                { 
                    legal = true;
                    Vector3 p = GetIntersection(faces[i].n,faces[j].n,faces[k].n,faces[i].d,faces[j].d,faces[k].d);
                    if (p != new Vector3(-999999, 0, 0))
                    {
                        for (int m = 0; m < faces.Length; m++)
                        {
                            if (Vector3.Dot(p, faces[m].n) + faces[m].d > 0)
                            {
                                legal = true;
                            }

                            if (legal)
                            {
                                polys[i].AddVertex(p, faces[i].n);
                                polys[j].AddVertex(p, faces[j].n);
                                polys[k].AddVertex(p, faces[k].n);
                            }
                        }
                    }
                }
            }
        }
    }

    Vector3 GetIntersection (Vector3 n1, Vector3 n2, Vector3 n3, float d1, float d2, float d3)
    {
        float denom = Vector3.Dot(n1,Vector3.Cross(n2,n3));

        if (denom == 0) return new Vector3(-999999, 0, 0);
        Vector3 p = ((-d1 * Vector3.Cross(n2, n3)) - (d2 * Vector3.Cross(n3,n1)) - (d3 * Vector3.Cross(n1,n2))) / denom;
        //Vector3 p = ((-d1 * Vector3.Cross(n2, n3)) - (d2 * Vector3.Cross(n3, n1)) - (d3 * Vector3.Cross(n1, n2))) / denom;
        return p;
    }

    public void MakeMesh()
    {
        Mesh mesh = new Mesh();
        List<Vector3> allVerts = new List<Vector3>();

        foreach(Poly p in polys)
        {
            p.Order();
            for (int i = 0; i < p.verts.Count; i++)
            {
                allVerts.Add(p.verts[i]);
            }
        }

        List<int> tris = new List<int>();
        int triCount = 0;

        for (int i = 0; i < polys.Length; i++)
        {
            for (int j = 0; j < polys[i].tris.Length; j++)
            {
                tris.Add(polys[i].tris[j] + triCount);
            }
            triCount += 4;
        }


        Vector2[] uvs = new Vector2[allVerts.Count];
        for (int i = 0; i < allVerts.Count; i++)
        {
            uvs[i] = new Vector2(allVerts[i].x, allVerts[i].z);
        }

        mesh.vertices = allVerts.ToArray();
        meshFilter.mesh = mesh;
        mesh.triangles = tris.ToArray();
        mesh.uv = uvs;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        
        int sphereCount = 0;
        foreach (Vector3 v in allVerts)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.name = sphereCount.ToString();
            sphereCount++;
            sphere.transform.position = v;
            sphere.transform.SetParent(transform);
        }
    }
}
