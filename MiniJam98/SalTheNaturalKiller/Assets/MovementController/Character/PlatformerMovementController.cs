using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

namespace KMGMovement2D {
    [RequireComponent(typeof(CharacterMover2D))]
    public class PlatformerMovementController : MonoBehaviour, IMovementUser {
        public Vector2 drive;
        public bool jump;

        [Header("Movement")]
        public float movementSpeed;

        [Header("Physics")]
        public Vector2 gravityDirection;
        public float gravityStrength;

        [Header("Slopes and Steps")]
        [Range(0, 1)] public float airborneBounce;
        [Range(0, 90)] public float maxSlopeAngle;
        public float stepDownHeight;
        public float stepUpHeight;


        private CharacterMover2D _characterMover;
        [ReadOnly] public Vector2 velocity;
        [ReadOnly] public bool isGrounded = true;
        [ReadOnly] public Collider2D currentSurface;
        [ReadOnly] public Vector2 currentSurfaceNormal;

        private void Awake() {
            _characterMover = GetComponent<CharacterMover2D>();
            isGrounded = false;
            currentSurfaceNormal = -gravityDirection;
        }

        public void DebugReset() {
            currentSurfaceNormal = -gravityDirection;
            velocity = Vector2.zero;
            isGrounded = false;
        }

        public void DoTick() {
            if (isGrounded) {
                DoGroundedBehavior();
                _characterMover.RecordDebug(Color.cyan, "Grounded.");
            } else {
                DoAirborneBehavior();
                _characterMover.RecordDebug(Color.Lerp(Color.red, Color.yellow, 0.5f), "Airborne.");
            }
        }

        private void DoGroundedBehavior() {
            Vector2 movement = drive.normalized * movementSpeed * Time.fixedDeltaTime;
            movement  = Vector3.ProjectOnPlane(movement, currentSurfaceNormal);
            velocity = movement/Time.fixedDeltaTime;
            var hit = _characterMover.Displace(movement.normalized, movement.magnitude);
            if (hit) {
                if (IsSurfaceWalkable(hit.normal)) {
                    currentSurfaceNormal = hit.normal;
                    currentSurface = hit.collider;
                } else {
                    // Find ground below
                    var groundHit = _characterMover.Displace(-currentSurfaceNormal, 0.0f);
                    if(groundHit && !IsSurfaceWalkable(groundHit.normal)) {
                        float angle = Vector2.SignedAngle(groundHit.normal, currentSurfaceNormal) * Mathf.Deg2Rad;
                        float xDist = groundHit.distance / Mathf.Tan(angle);
                        Vector2 backDisp = currentSurfaceNormal.normalized * groundHit.distance + Vector2.Perpendicular(currentSurfaceNormal).normalized * xDist;
                        _characterMover.Move(backDisp);
                    }
                }
            }
            //TryStepDown();
        }

        private void TryStepDown() {
            var lastPos = _characterMover.characterPosition;
            var ground = _characterMover.Displace(gravityDirection, stepDownHeight);
            if (ground && IsSurfaceWalkable(ground.normal)) {
                currentSurfaceNormal = ground.normal;
                currentSurface = ground.collider;
                isGrounded = true;
            } else {
                _characterMover.Move(lastPos, false);
                isGrounded = false;
            }
        }

        private void DoAirborneBehavior() {
            velocity += gravityDirection * gravityStrength * Time.fixedDeltaTime;
            _characterMover.DisplaceAndSlide(velocity, velocity.magnitude * Time.fixedDeltaTime, delegate(RaycastHit2D hit, ref Vector2 remainingDisplacement) {
                if (IsSurfaceWalkable(hit.normal)) {
                    currentSurface = hit.collider;
                    currentSurfaceNormal = hit.normal;
                    isGrounded = true;
                    velocity = Vector2.zero;
                    remainingDisplacement = Vector2.zero;
                    return;
                }
                Vector2 projected = Vector3.ProjectOnPlane(velocity, hit.normal);
                Vector2 bounced = Vector2.Reflect(velocity, hit.normal);
                velocity = Vector2.Lerp(projected, bounced, airborneBounce);
            });
        }

        private bool IsSurfaceWalkable(Vector2 surfaceNormal) {
            return Vector2.Angle(-gravityDirection, surfaceNormal) <= maxSlopeAngle;
        }
    }
}
