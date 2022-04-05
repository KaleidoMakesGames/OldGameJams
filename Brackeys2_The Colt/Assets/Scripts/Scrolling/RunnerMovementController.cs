using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RunnerMovementController : MonoBehaviour
{
    public float jumpForce;
    public float hangForce;

    public UnityEvent OnJump;
    public UnityEvent OnLand;

    private bool isHanging;
    private bool isJumping;
    private Rigidbody2D rb;
    
    private void Awake() {
        isHanging = false;
        isJumping = false;
        rb = GetComponent<Rigidbody2D>();
    }

    private bool hasSpaceBeenUp = false;

    private void LateUpdate() {
        if(!Input.GetKey(KeyCode.Space)) {
            hasSpaceBeenUp = true;
        }
    }

    private void FixedUpdate() {
        if(!hasSpaceBeenUp) {
            return;
        }
        Runner.Instance.runningVector.y = rb.velocity.y / 2.0f;
        if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)) {
            Jump();
        } else {
            isHanging = false;
        }
    }

    public void Jump() {
        if (!isJumping) {
            rb.AddForce(Vector2.up * jumpForce);
            isJumping = true;
            isHanging = true;
            OnJump.Invoke();
        }
        if (isHanging) {
            rb.AddForce(Vector2.up * hangForce);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.GetComponent<Ground>() != null) {
            if (isJumping) {
                OnLand.Invoke();
            }
            isJumping = false;
        }
    }
}
