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
    [ReadOnly] public bool isFalling;
    [ReadOnly] public Vector2 respawnPosition;

    // Update is called once per frame
    void Update() {
        if (!isFalling) {
            var touchingPit = movementController.characterMover.GetTouchingObjects().Any(x => x.GetComponentInParent<Pit>() != null);
            if (!touchingPit) {
                respawnPosition = movementController.characterMover.characterPosition;
            }
            if(!movementController.isDashing && !movementController.FindFloor()) {
                DoFall();
            }
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
    }

    private IEnumerator DoRespawn() {
        Vector2 startPosition = transform.position;
        float startTime = Time.time;
        while(true) {
            float progress = (Time.time - startTime) / respawnTime;
            if(progress >= 1.0f) {
                break;
            }
            transform.position = Vector2.Lerp(startPosition, respawnPosition, respawnCurve.Evaluate(progress));
            yield return null;
        }
        transform.position = respawnPosition;
        movementController.characterMover.Move(respawnPosition, true);
        isFalling = false;
        movementController.enabled = true;
        movementController.characterMover.enabled = true;
    }
}