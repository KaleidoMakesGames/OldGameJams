using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FloorZoomer : MonoBehaviour
{
    public PlayArea area;
    public Transform minMarker;
    public Transform maxMarker;
    public void Update() {
        Bounds b = area.bounds;
        minMarker.position = b.min;
        maxMarker.position = b.max;
    }
}
