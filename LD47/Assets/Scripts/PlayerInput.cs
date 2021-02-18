using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public PathDrawer pathDrawer;
    public CircleCollider2D clickPoint;

    [ReadOnly] public bool tracing;

    // Update is called once per frame
    void Update() {
        bool shouldTrace = Input.GetButton("Trace");
        Vector2 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (tracing) {
            pathDrawer.SetCurrentPoint(currentMousePosition);

            if(!shouldTrace) {
                pathDrawer.ClearPath();
                tracing = false;
            }
        } else {
            if (!pathDrawer.isClearing && shouldTrace) {
                if (clickPoint.OverlapPoint(currentMousePosition)) {
                    tracing = true;
                    pathDrawer.SetCurrentPoint(clickPoint.transform.position);
                }
            }
        }
    }
}
