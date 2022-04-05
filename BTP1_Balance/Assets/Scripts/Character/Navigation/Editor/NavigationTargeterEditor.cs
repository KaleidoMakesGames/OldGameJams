using System.Collections;
using UnityEngine;
using UnityEditor;

namespace Navigation {
    [CustomEditor(typeof(NavigationTargeter))]
    public class NavigationTargeterEditor : Editor {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            NavigationTargeter myScript = (NavigationTargeter)target;
            if (GUILayout.Button("Recompute Path To Target")) {
                myScript.RecomputeWaypoints();
            }
        }
    }
}