using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class PlayerMovementController : MonoBehaviour {
    [Header("Control")]
    public Vector2 drive;

    [Header("Collision")]
    public CapsuleCollider bumper;
    public SphereCollider riser;
    public float groundStickDistance;
    public float groundCheckDistance;

    [Header("Movement")]
    public float movementSpeed;
    
    [Header("Gravity and Jumping")]
    public float gravity;
    [Range(0.0f, 1.0f)] public float airborneMovementInfluence;
    
    [Header("Status")]
    [ReadOnly] public bool isAirborne;
    [ReadOnly] public Vector3 velocity;
    [ReadOnly] public Vector3 acceleration;

    public Vector3 groundVelocity {
        get {
            Vector3 g = velocity;
            g.y = 0.0f;
            return g;
        }
    }
    public RaycastHit groundHit { get; private set; }

    // PRIVATE
    private Rigidbody _rb;

    private Rigidbody _movingFloorRB;
    private Vector3 _lastMovingFloorRBPosition;
    
    private float _verticalVelocity;

    private Vector3 _currentFloorVelocity;
    private Vector3 _lastPosition;
    private Vector3 _lastVelocity;

    
    private Vector3 bumperWorldA {
        get {
            return transform.TransformPoint(bumper.center + Vector3.up * (bumper.height / 2.0f - bumper.radius));
        }
    }
    private Vector3 bumperWorldB {
        get {
            return transform.TransformPoint(bumper.center - Vector3.up * (bumper.height / 2.0f - bumper.radius));
        }
    }

    private void Awake() {
        _rb = GetComponent<Rigidbody>();
        _lastPosition = transform.position;
        isAirborne = false;
    }
    
    private void FixedUpdate() {
        _rb.isKinematic = true;
        
        Vector3 movementDelta = Vector3.zero;
        ApplyDrive(ref movementDelta);
        UpdateCollisions(ref movementDelta);
        if (isAirborne) {
            ApplyVerticalVelocity(ref movementDelta);
        } else {
            StickToGround(ref movementDelta);
        }
        ApplyGroundPlatformDelta(ref movementDelta);
        UpdateCollisions(ref movementDelta);
        _rb.MovePosition(_rb.position + movementDelta);
        UpdateStatus();
    }
    
    public void SetVerticalVelocity(float v) {
        if (v > 0) {
            isAirborne = true;
            _verticalVelocity = v;
        }
    }

    public float GetVerticalVelocity() {
        if (isAirborne) {
            return _verticalVelocity;
        } else {
            return 0.0f;
        }
    }

    private void ApplyGroundPlatformDelta(ref Vector3 movementDelta) {
        if(_movingFloorRB != null) {
            movementDelta += _movingFloorRB.position - _lastMovingFloorRBPosition;
            _lastMovingFloorRBPosition = _movingFloorRB.position;
        }
    }

    private void ApplyDrive(ref Vector3 currentDelta) {
        Vector3 movementVector = new Vector3(drive.x, 0.0f, drive.y);

        movementVector = Vector3.ClampMagnitude(movementVector * movementSpeed, movementSpeed);
        
        if (isAirborne) {
            _currentFloorVelocity = (airborneMovementInfluence) * movementVector + (1 - airborneMovementInfluence) * _currentFloorVelocity;
        } else {
            _currentFloorVelocity = movementVector;
        }

        currentDelta += _currentFloorVelocity * Time.fixedDeltaTime;
    }

    private void UpdateCollisions(ref Vector3 currentDelta) {
        foreach (Collider c in Physics.OverlapCapsule(bumperWorldA + currentDelta, bumperWorldB + currentDelta, bumper.radius)) {
            var otherCharacterMovementController = c.GetComponentInParent<PlayerMovementController>();

            if(ShouldIgnoreCollider(c)) {
                continue;
            }

            if (c.GetComponentInParent<Rigidbody>() == null || c.GetComponentInParent<Rigidbody>().isKinematic) {
                // Resolve collisions with any static colliders. Treat them like solid walls.
                MoveOutOfCollider(ref currentDelta, c);
            }
        }
    }

    private Vector3 MoveOutOfCollider(ref Vector3 currentDelta, Collider otherCollider, bool reverse = false) {
        float d;
        Vector3 movement;
        if (Physics.ComputePenetration(bumper, transform.position + currentDelta, transform.rotation, otherCollider, otherCollider.transform.position, otherCollider.transform.rotation, out movement, out d)) {
            movement = movement.normalized * d;
            currentDelta += movement;
            // If we hit something above us, set our vertical velocity to downwards
            if (isAirborne && Vector3.Angle(movement.normalized, Vector3.down) <= 45.0f) {
                _verticalVelocity = Mathf.Clamp(_verticalVelocity, Mathf.NegativeInfinity, 0.0f);
            }
            return movement;
        }
        return Vector3.zero;
    }
    
    private void StickToGround(ref Vector3 currentDelta) {
        // Start at the riser
        Vector3 riserStart = bumperWorldA + currentDelta;
        // End at the riser spot plus stick distance
        Vector3 riserEnd = transform.TransformPoint(riser.center + Vector3.down * groundStickDistance) + currentDelta;

        Vector3 castVector = riserEnd - riserStart;

        groundHit = new RaycastHit();
        foreach (RaycastHit h in Physics.SphereCastAll(riserStart, riser.radius, castVector, castVector.magnitude)) {
            if (ShouldIgnoreCollider(h.collider)) {
                continue;
            }
            if (groundHit.collider == null || h.distance < groundHit.distance) {
                groundHit = h;
            }
        }

        if (groundHit.collider != null) {
            // Stick ourselves to it
            Vector3 riserHitPoint = riserStart + castVector.normalized * groundHit.distance;
            currentDelta += riserHitPoint - (transform.TransformPoint(riser.center) + currentDelta);
            _verticalVelocity = 0.0f;

            var hitRB = groundHit.collider.GetComponentInParent<Rigidbody>();
            if (hitRB == null || !hitRB.isKinematic) {
                _movingFloorRB = null;
            } else {
                if (hitRB != _movingFloorRB) {
                    _lastMovingFloorRBPosition = hitRB.position;
                }
                _movingFloorRB = hitRB;
            }
        } else {
            // We are airborne
            isAirborne = true;
        }
    }


    private void ApplyVerticalVelocity(ref Vector3 currentDelta) {
        _verticalVelocity += gravity * Time.fixedDeltaTime;

        currentDelta += _verticalVelocity * Time.fixedDeltaTime * Vector3.up;
        
        if (_verticalVelocity <= 0.0f) {
            if (IsGrounded(ref currentDelta)) {
                isAirborne = false;
            }
        }
    }

    private bool IsGrounded(ref Vector3 currentDelta) {
        // Start at the riser
        Vector3 riserStart = bumperWorldA + currentDelta;
        // End at the riser spot plus stick distance
        Vector3 riserEnd = transform.TransformPoint(riser.center + Vector3.down * groundCheckDistance) + currentDelta;

        Vector3 castVector = riserEnd - riserStart;

        foreach (RaycastHit h in Physics.SphereCastAll(riserStart, riser.radius, castVector, castVector.magnitude)) {
            if (ShouldIgnoreCollider(h.collider)) {
                continue;
            }
            return true;
        }
        return false;
    }

    private bool ShouldIgnoreCollider(Collider c) {
        if (Physics.GetIgnoreLayerCollision(gameObject.layer, c.gameObject.layer)) {
            return true;
        }
        if (c.GetComponentInParent<PlayerMovementController>() == this || c.isTrigger || (c.GetComponentInParent<Rigidbody>() != null && !c.GetComponentInParent<Rigidbody>().isKinematic)) {
            return true;
        }
        return false;
    }

    private void UpdateStatus() {
        velocity = (transform.position - _lastPosition) / Time.fixedDeltaTime;
        if (velocity.magnitude <= Physics.sleepThreshold) {
            velocity = Vector3.zero;
        }
        acceleration = velocity - _lastVelocity;

        _lastVelocity = velocity;
        _lastPosition = transform.position;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(transform.TransformPoint(riser.center), riser.radius);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.TransformPoint(riser.center) + Vector3.down * groundStickDistance, riser.radius);
    }
}
