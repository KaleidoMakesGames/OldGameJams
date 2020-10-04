using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementController : MonoBehaviour {
    public Rigidbody2D rb;

    public float baseMovementSpeed;
    public float movementAcceleration;

    public Vector2 movementDrive;

    private void FixedUpdate() {
        rb.velocity = Vector2.MoveTowards(rb.velocity, movementDrive* baseMovementSpeed, movementAcceleration * Time.fixedDeltaTime);
    }
}
