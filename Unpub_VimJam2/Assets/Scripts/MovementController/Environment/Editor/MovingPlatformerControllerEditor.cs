using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace KMGMovement2D {
    [CustomEditor(typeof(MovingPlatformController))]
    public class MovingPlatformerControllerEditor : Editor {
        int selectedWaypoint;

        private void OnSceneGUI() {
            var platformController = target as MovingPlatformController;

            for (int i = 1; i < platformController.waypoints.Count; i++) {
                var worldPoint = platformController.ToWorld(platformController.waypoints[i].localPosition);
                float size = HandleUtility.GetHandleSize(worldPoint) * 0.05f;

                if (selectedWaypoint == i) {
                    using (new Handles.DrawingScope(Color.white)) {
                        Handles.Button(worldPoint, Quaternion.identity, size, size, Handles.DotHandleCap);
                        Handles.DrawWireCube(platformController.ToWorld(platformController.waypoints[i].localPosition) +
                            (Vector2)platformController.colliderLocalBounds.center, platformController.colliderLocalBounds.size);
                    }
                    var newPoint = Handles.PositionHandle(worldPoint, Quaternion.identity);
                    platformController.waypoints[i].localPosition = platformController.ToLocal(newPoint);
                } else {
                    using (new Handles.DrawingScope(Color.blue)) {
                        Handles.Button(worldPoint, Quaternion.identity, size, size, Handles.DotHandleCap);
                        if (Handles.Button(worldPoint, Quaternion.identity, size, size, Handles.DotHandleCap)) {
                            selectedWaypoint = i;
                        }
                        Handles.DrawWireCube(platformController.ToWorld(platformController.waypoints[i].localPosition) +
                            (Vector2)platformController.colliderLocalBounds.center, platformController.colliderLocalBounds.size);
                    }
                }
            }

            for (int i = 0; i < platformController.waypoints.Count-1; i++) {
                var a = platformController.waypoints[i];
                var b = platformController.waypoints[i + 1];
                Handles.DrawDottedLine(platformController.ToWorld(a.localPosition),
                    platformController.ToWorld(b.localPosition), 2.0f);
            }
        }
    }
}