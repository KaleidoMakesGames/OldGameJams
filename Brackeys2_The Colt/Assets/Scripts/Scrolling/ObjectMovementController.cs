using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovementController : MonoBehaviour
{
    public float distanceFromRunner;
    public Vector2 constantVelocity;

    public bool moveX = true;
    public bool moveY = true;

    private void FixedUpdate() {
        if (Runner.Instance.enabled) {
            Vector2 movement = -Runner.Instance.runningVector * (1 / (distanceFromRunner + 1.0f)) * Time.fixedDeltaTime;
            movement.x = moveX ? movement.x : 0.0f;
            movement.y = moveY ? movement.y : 0.0f;
            transform.Translate(movement);
        }
        transform.Translate(constantVelocity * Time.fixedDeltaTime);
    }
}
