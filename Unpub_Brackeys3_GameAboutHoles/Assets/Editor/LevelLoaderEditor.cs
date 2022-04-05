using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelLoader))]
public class LevelLoaderEditor : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        LevelLoader l = (LevelLoader)target;
        if(GUILayout.Button("Reload Level")) {
            l.ClearLevel(!EditorApplication.isPlaying);
            l.LoadLevel();
        }
    }
}
