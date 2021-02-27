using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KCharacterController : MonoBehaviour
{
    public Vector2Int drive;
    public float movementSpeed;
    public Rigidbody2D rb;
    public float delay;

    private void FixedUpdate() {
        float maxDelta = delay <= 0 ? Mathf.Infinity : Time.fixedDeltaTime / delay;
        rb.velocity = Vector2.MoveTowards(rb.velocity, (Vector2)drive * movementSpeed, maxDelta);
    }

    private void OnValidate() {
        delay = Mathf.Max(delay, 0);
    }
}
