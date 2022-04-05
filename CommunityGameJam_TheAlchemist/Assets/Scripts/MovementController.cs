using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovementController : MonoBehaviour
{
    public Rigidbody2D characterRigidbody;

    public float movementSpeed = 1.0f;

    public UnityEvent OnMove;

    private void Update() {
        Vector2 movement = Vector2.zero;
        if(Input.GetKey(KeyCode.W)) {
            movement += Vector2.up;
        }
        if (Input.GetKey(KeyCode.S)) {
            movement += Vector2.down;
        }
        if (Input.GetKey(KeyCode.A)) {
            movement += Vector2.left;
        }
        if (Input.GetKey(KeyCode.D)) {
            movement += Vector2.right;
        }

        if(movement != Vector2.zero) {
            transform.up = movement;
            OnMove.Invoke();
        }

        DoMovement(movement.normalized);
    }

    private void DoMovement(Vector2 movement) {
        characterRigidbody.velocity = movement * movementSpeed;
    }
}
