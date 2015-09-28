using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEditor;

public class QuakeMapImporter : MonoBehaviour
{

    static public void Generate(string content)
    {
        GameObject container;
        container = GameObject.Find("QuakeMapContainer");
        if (container) DestroyImmediate(container);

        container = new GameObject("QuakeMapContainer");

        MakeBrush(Parse(content), container.transform);
    }

    static List<Vector3> Parse(string content)
    {
        content = content.Remove(0, 50);
        content = content.Remove(content.Length - 5 - 1, 5);

        string[] data = content.Split('\n');
        List<string> tempVectorsList = new List<string>();
        List<Vector3> vectors = new List<Vector3>();

        for (int i = 0; i < data.Length; i++)
        {
            data[i] = data[i].Replace("(", "");
            data[i] = data[i].Replace(")", "");
            int pos = data[i].IndexOf("_");
            if (pos > 0)
            {
                data[i] = data[i].Remove(pos);
                data[i] = data[i].Replace("   ", "*");
                string [] tempVectors = data[i].Split('*');
                foreach(string s in tempVectors){
                    if (s.Length > 0) tempVectorsList.Add(s);
                }
            }
        }

        foreach (string s in tempVectorsList)
        {
            vectors.Add(Convert(s));
        }

        return vectors;
    }

    static public void MakeBrush(List<Vector3> vectors, Transform container)
    {
        int brushes = vectors.Count / 6 / 3;

        int curVert = 0;

        for (int i = 0; i < brushes; i++)
        {
            GameObject brushGO = new GameObject("brush_" + i.ToString());
            Brush brush = brushGO.AddComponent<Brush>();

            Face[] faces = new Face[6];
            for (int j = 0; j < 6; j++)
            {
                faces[j] = new Face();
                faces[j].p0 = vectors[curVert];
                curVert++;
                faces[j].p1 = vectors[curVert];
                curVert ++;
                faces[j].p2 = vectors[curVert];
                curVert ++;
            }

            brush.SetUp(container, faces);
        }
        

        container.rotation = Quaternion.Euler(90, 0, 0);
    }

    public static Vector3 Convert(string s){
        Vector3 v = Vector3.zero;
        string[] array = s.Split(' ');
        if (array[0] != "") 
        {
            v.x = float.Parse(array[0]);
            v.y = float.Parse(array[1]);
            v.z = float.Parse(array[2]);
        }
        else
        {
            v.x = float.Parse(array[1]);
            v.y = float.Parse(array[2]);
            v.z = float.Parse(array[3]);
        }
        return v;
    }
}
