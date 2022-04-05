using System.Collections;
using UnityEngine;
using UnityEditor;

namespace Navigation {
    [CustomEditor(typeof(NavigatableAreaPathfinder))]
    public class NavigatableAreaTraverserEditor : Editor {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            NavigatableAreaPathfinder myScript = (NavigatableAreaPathfinder)target;
            if (GUILayout.Button("Recompute Nodes")) {
                myScript.RecomputeNodes();
            }
        }
    }
}