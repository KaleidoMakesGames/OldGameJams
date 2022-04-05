using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovementController : MonoBehaviour {
    public float movementSpeed;
    public float airMovementSpeed;
    public float jumpForce;
    public float jumpHangForce;
    public float jumpDecayRate;
    public float gravity;

    public float slowedTime;
    public float slowedTimeLerp;

    public float dashSpeed;
    public float dashDistance;

    public float recoveryTime;
    public Vector2 knockbackForce;

    public float groundedDistance;

    public bool facingLeft { get; private set; }
    
    public bool infinityJump = false;
    public bool canDash = false;

    private bool hanging = false;
    private float hangStrength = 0.0f;

    private bool isKnockedBack = false;
    private float knockbackTime = Mathf.NegativeInfinity;

    private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if(PlayerChoices.Instance.canSlowTime) {
            float goalTime = 1.0f;
            if(Input.GetKey(KeyCode.LeftControl) || Input.GetMouseButton(1)) {
                goalTime = slowedTime;
            }
            Time.timeScale = Mathf.MoveTowards(Time.timeScale, goalTime, slowedTimeLerp * Time.deltaTime);
        }

        float hVelocity = 0.0f;

        if (isKnockedBack) {
            if (Time.time - knockbackTime >= recoveryTime) {
                isKnockedBack = false;
            } else {
                hVelocity = rb.velocity.x;
            }
        } else {
            if (Input.GetKey(KeyCode.A)) {
                hVelocity -= movementSpeed;
                facingLeft = true;
            }

            if (Input.GetKey(KeyCode.D)) {
                hVelocity += movementSpeed;
                facingLeft = false;
            }

            DoJump();

            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash) {
                Vector2 moveDirection = (facingLeft ? Vector2.left : Vector2.right);
                float moveDistance = dashDistance;
                float radius = GetComponent<Collider2D>().bounds.size.x / 2.0f;
                bool anythingAtEnd = false;
                foreach (Collider2D collider in Physics2D.OverlapPointAll((Vector2)transform.position + moveDirection.normalized * dashDistance)) {
                    if (!collider.isTrigger && collider.gameObject != gameObject) {
                        anythingAtEnd = true;
                    }
                }
                if (anythingAtEnd) {
                    foreach (RaycastHit2D hit in Physics2D.CircleCastAll(transform.position, radius, moveDirection.normalized, dashDistance)) {
                        if (!hit.collider.isTrigger && hit.collider.gameObject != gameObject) {
                            moveDistance = hit.distance;
                        }
                    }
                }
                rb.transform.position = (Vector2)transform.position + moveDirection * moveDistance;
            }

            if (!IsGrounded()) {
                hVelocity *= airMovementSpeed;
            }
        }

        rb.velocity = new Vector2(hVelocity, rb.velocity.y - gravity * Time.deltaTime);
    }

    private bool IsGrounded() {
        foreach (RaycastHit2D hit in Physics2D.RaycastAll(transform.position, Vector2.down, groundedDistance)) {
            if (hit.collider.GetComponent<JumpResetter>() != null) {
                return true;
            }
        }
        return false;
    }
    private void DoJump() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            bool canJump = false;
            if (IsGrounded() || infinityJump) {
                canJump = true;
            }
            if (canJump) {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                hangStrength = jumpHangForce;
                hanging = true;
            }
        }

        if(Input.GetKeyUp(KeyCode.Space)) {
            hanging = false;
        }

        if(hanging) {
            rb.velocity += Vector2.up * hangStrength * Time.deltaTime;
            hangStrength = Mathf.Clamp(hangStrength - Time.deltaTime * jumpDecayRate * hangStrength, 0.0f, Mathf.Infinity);
        }
    }

    public void DoKnockback() {
        if(isKnockedBack) {
            return;
        }

        isKnockedBack = true;
        knockbackTime = Time.time;

        Vector2 force = new Vector2(knockbackForce.x * (facingLeft ? 1.0f : -1.0f), knockbackForce.y);

        rb.velocity = Vector2.zero;
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundedDistance);
    }
}
