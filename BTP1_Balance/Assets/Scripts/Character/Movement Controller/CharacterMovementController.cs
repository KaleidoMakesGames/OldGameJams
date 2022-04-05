using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character {
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterMovementController : MonoBehaviour {
        public float movementSpeed;
        public float behaviorVelocityThreshold;

        public MovementBehavior behavior;

        private Rigidbody2D rb;

        private void Awake() {
            rb = GetComponent<Rigidbody2D>();
        }

        public void MoveInDirection(Vector2 direction) {
            rb.MovePosition(rb.position + direction.normalized * movementSpeed * Time.fixedDeltaTime);
        }

        public void MoveByDelta(Vector2 deltaMove) {
            Vector2 nextRBPosition = rb.position + deltaMove;
            rb.MovePosition(nextRBPosition);
        }
        
        private void FixedUpdate() {
            if (rb.velocity.magnitude <= behaviorVelocityThreshold) {
                rb.velocity = Vector2.zero;
            } else {
                return;
            }

            if (behavior != null) {
                behavior.DoBehavior(this);
            }
        }
    }
}
