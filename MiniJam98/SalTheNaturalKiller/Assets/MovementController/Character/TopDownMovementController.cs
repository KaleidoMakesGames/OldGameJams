using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KMGMovement2D {
    [RequireComponent(typeof(CharacterMover2D))]
    public class TopDownMovementController : MonoBehaviour, IMovementUser {
        [Header("Control")]
        public Vector2 drive;
        public bool dash;

        [Header("Movement")]
        public float movementSpeed;
        public float dashDistance;
        public float dashTime;
        public float dashCooldown;
        public AnimationCurve dashCurve;
        [ReadOnly] public bool isDashing;
        [ReadOnly] public float dashCooldownRemaining;
        private float _lastDashTime;

        [Header("Pushing")]
        public float pushForce;

        public CharacterMover2D characterMover { get; private set; }
        [ReadOnly] public FloorProvider currentFloor;
        [ReadOnly][SerializeField]private Vector2 velocity;

        private void Awake() {
            characterMover = GetComponent<CharacterMover2D>();
        }

        public void DebugReset() {
            _lastDashTime = Mathf.NegativeInfinity;
            isDashing = false;
        }

        public void DoTick() {
            if (!isDashing) {
                DoMovement();
                FindFloor();
                if (dash && drive.magnitude > 0 && Time.time - _lastDashTime >= dashCooldown) {
                    StartCoroutine(DoDash());
                }
            }
        }

        private void FindFloor() {
            var touchingObjects = characterMover.GetTouchingObjects();
            currentFloor = null;
            foreach (var touchingObject in touchingObjects) {
                var floor = touchingObject.GetComponentInParent<FloorProvider>();
                if (floor) {
                    currentFloor = floor;
                    break;
                }
            }
        }

        private void Update() {
            dashCooldownRemaining = Mathf.Max(0, dashCooldown - (Time.time - _lastDashTime));
        }

        private IEnumerator DoDash() {
            Vector2 dashVector = drive.normalized;
            Vector2 startPosition = characterMover.characterPosition;
            Vector2 goalPosition = characterMover.characterPosition + dashVector * dashDistance;
            float startTime = Time.fixedTime;
            _lastDashTime = Time.fixedTime;
            isDashing = true;
            while(isDashing) {
                float progress = (Time.fixedTime - startTime)/dashTime;
                if(progress >= 1) {
                    characterMover.DisplaceTo(goalPosition);
                    break;
                }
                Vector2 localGoal = Vector2.LerpUnclamped(startPosition, goalPosition, dashCurve.Evaluate(progress));
                bool hit = false;
                characterMover.DisplaceAndSlideTo(localGoal, delegate(RaycastHit2D h, ref Vector2 remainingDisplacement) {
                    hit = true;
                    remainingDisplacement = Vector2.zero;
                });
                if(hit) {
                    break;
                }
                yield return new WaitForFixedUpdate();
            } 
            isDashing = false;
        }

        private void DoMovement() {
            Vector2 goalVelocity = drive.normalized * movementSpeed;
            float damping = currentFloor ? 1 - currentFloor.friction : 0;
            Vector2 dampedVelocity = Vector2.Lerp(goalVelocity, velocity, damping);
            Vector2 lastPosition = characterMover.characterPosition;
            characterMover.DisplaceAndSlide(dampedVelocity, dampedVelocity.magnitude * Time.fixedDeltaTime, delegate(RaycastHit2D hit, ref Vector2 remainingDisplacement) {
                var rb = hit.collider.GetComponent<Rigidbody2D>();
                if (rb && !rb.isKinematic) {
                    rb.AddForceAtPosition(-hit.normal * pushForce, hit.point, ForceMode2D.Force);
                    return;
                }
            });

            Vector2 newPosition = characterMover.characterPosition;
            velocity = (newPosition - lastPosition) / Time.fixedDeltaTime;
        }
    }
}