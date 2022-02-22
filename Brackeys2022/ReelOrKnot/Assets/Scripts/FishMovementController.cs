using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMovementController : MonoBehaviour
{
    public float movementSpeed;

    public Rigidbody2D rb;

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 goalPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        rb.velocity = Vector2.ClampMagnitude(goalPosition - rb.position, 1.0f) * movementSpeed;
    }
}
