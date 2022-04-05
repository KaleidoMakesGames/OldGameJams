using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollImage : MonoBehaviour {
    public float scrollDistance;

    public float scrollTime;

    private Vector2 velocity;

    private Vector2 goalPosition;

    private void Awake() {
        goalPosition = Vector2.zero;
    }

    private void Update() {
        transform.position = (Vector3)(Vector2.SmoothDamp(transform.position, goalPosition, ref velocity, scrollTime)) + Vector3.forward * transform.position.z;
    }

    public void DoScroll() {
        goalPosition += Vector2.right * scrollDistance;
    }
}
