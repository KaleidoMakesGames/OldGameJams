using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayArea : MonoBehaviour
{
    [HideInInspector] public Bounds bounds;

    public void UpdateBounds() {
        bounds = new Bounds();
        bool initialized = false;
        foreach (Collider c in GetComponentsInChildren<Collider>()) {
            if (initialized) {
                bounds.Encapsulate(c.bounds);
            } else {
                bounds = c.bounds;
                initialized = true;
            }
        }
        bounds.size += Vector3.one * 0.5f;
    }
}
