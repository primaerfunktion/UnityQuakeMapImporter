using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.IO;

public class MenuBar : EditorWindow
{
    bool groupEnabled;
    string mapName = "testMap";

    [MenuItem("Quake.mapImporter/Import")]
    static void Init()
    {
         MenuBar window = (MenuBar)EditorWindow.GetWindow(typeof(MenuBar));
         window.titleContent = new GUIContent("Quake.mapImporter");
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Mapname:");
        mapName = GUILayout.TextField(mapName);
        GUILayout.EndHorizontal();

        if(GUILayout.Button("Generate")){
            StreamReader sr = new StreamReader(Application.dataPath + "/" + "QuakeMapImporter" + "/" + "maps" + "/" + mapName + ".map");
            string content = sr.ReadToEnd();
            sr.Close();

            Import(content);
        }
    }

    static void Import(string content)
    {
        QuakeMapImporter.Generate(content);
    }
}
