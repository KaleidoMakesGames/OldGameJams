using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KMGMovement2D;

[RequireComponent(typeof(CharacterMover2D))]
public class TopDownMovementController : MonoBehaviour {
    [Header("Control")]
    public Vector2 drive;

    [Header("Movement")]
    public float movementSpeed;

    [Header("Attack Dash")]
    public float dashSpeed;
    public AnimationCurve dashCurve;
    [ReadOnly] public bool isDashing;

    public CharacterMover2D characterMover { get; private set; }
    [ReadOnly] public FloorProvider currentFloor;
    [ReadOnly][SerializeField]private Vector2 velocity;

    private void Awake() {
        characterMover = GetComponent<CharacterMover2D>();
    }

    private void FixedUpdate() {
        if(!isDashing) {
            DoMovement();
            FindFloor();
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

    public void DashToPoint(Vector2 point) {
        StartCoroutine(DoDash(point));
    }

    private IEnumerator DoDash(Vector2 goalPosition) {
        Vector2 startPosition = characterMover.characterPosition;
        Vector2 dashVector = goalPosition - startPosition;

        float dashTime = dashVector.magnitude / dashSpeed;
        float startTime = Time.fixedTime;
        isDashing = true;
        while(isDashing) {
            float progress = Mathf.Clamp01((Time.fixedTime - startTime) / dashTime);
            if(progress == 1) {
                characterMover.TryMove(goalPosition);
                break;
            }
            Vector2 localGoal = Vector2.LerpUnclamped(startPosition, goalPosition, dashCurve.Evaluate(progress));
            var hit = characterMover.TryMove(localGoal, true);
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
        characterMover.DisplaceAndSlide(dampedVelocity * Time.fixedDeltaTime);
        Vector2 newPosition = characterMover.characterPosition;
        velocity = (newPosition - lastPosition) / Time.fixedDeltaTime;
    }
}