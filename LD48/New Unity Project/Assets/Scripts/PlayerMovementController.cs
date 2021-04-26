using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public Camera gameCamera;
    public float movementSpeed;
    public Rigidbody rb;

    private void FixedUpdate() {
        Vector3 dir = Vector3.zero;
        dir += Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) ? Vector3.forward : Vector3.zero;
        dir += Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) ? Vector3.back: Vector3.zero;
        dir += Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) ? Vector3.left : Vector3.zero;
        dir += Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) ? Vector3.right: Vector3.zero;

        if (dir != Vector3.zero) {
            float angle = Vector3.SignedAngle(Vector3.forward, dir, Vector3.up);

            dir = gameCamera.transform.forward;
            dir.y = 0.0f;
            dir= Quaternion.AngleAxis(angle, Vector3.up) * dir;
            dir.Normalize();
        }

        rb.velocity = dir * movementSpeed;
    }
}
