using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PlayerFloorDamager : MonoBehaviour
{
    public TopDownMovementController movementController;
    public Animator playerAnimator;
    public float respawnTime;
    public AnimationCurve respawnCurve;
    public UnityEvent OnFall;
    [ReadOnly] public bool isFalling;
    [ReadOnly] public Vector2 localRespawnPosition;
    [ReadOnly] public KMGMovement2D.FloorProvider respawnFloor;

    // Update is called once per frame
    void Update() {
        if (!isFalling && !movementController.isDashing) {
            TrackFloorRespawn();
        }
    }

    private void TrackFloorRespawn() {
        var currentFloor = movementController.FindFloor();
        if (currentFloor) {
            respawnFloor = currentFloor;
            var floorCollider = currentFloor.GetComponentInParent<Collider2D>();
            if(floorCollider.usedByComposite) {
                floorCollider = floorCollider.GetComponentInParent<CompositeCollider2D>();
            }
            var worldPoint = movementController.characterMover.characterPosition;
            if(!floorCollider.OverlapPoint(worldPoint)) {
                worldPoint = floorCollider.ClosestPoint(worldPoint);
            }
            localRespawnPosition = currentFloor.transform.InverseTransformPoint(worldPoint);
        } else {
            DoFall();
        }
    }

    private void DoFall() {
        isFalling = true;
        movementController.enabled = false;
        movementController.characterMover.enabled = false;
        playerAnimator.SetTrigger("Fall");
    }

    public void Respawn() {
        StartCoroutine(DoRespawn());
        OnFall.Invoke();
    }

    private IEnumerator DoRespawn() {
        Vector2 startPosition = transform.position;
        float startTime = Time.time;
        while(true) {
            var goalPosition = respawnFloor.transform.TransformPoint(localRespawnPosition);
            float progress = (Time.time - startTime) / respawnTime;
            if(progress >= 1.0f) {
                movementController.characterMover.Move(goalPosition, true);
                break;
            }
            transform.position = Vector2.Lerp(startPosition, goalPosition, respawnCurve.Evaluate(progress));
            yield return null;
        }
        isFalling = false;
        movementController.enabled = true;
        movementController.characterMover.enabled = true;
    }
}