using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterViewer : MonoBehaviour {
    [Range(0.0f, 3.0f)] public float size = 1.0f;
    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, size);
    }
}
