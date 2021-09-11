using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DashAttackController : MonoBehaviour
{
    [Header("Control")]
    public bool charge;
    public Vector2 desiredDashPosition;

    [Header("Settings")]
    public float dashMaxDistance;
    public float dashMinDistance;
    public float dashChargeTime;
    public float dashCooldownTime;

    [Header("References")]
    public TopDownMovementController movementController;

    [Header("State")]
    [ReadOnly] public bool isCharging;
    [ReadOnly] public float dashCooldownRemaining;
    [ReadOnly] public float chargeRadius;
    [ReadOnly] public float actualRadius;
    [ReadOnly] public Vector2 actualDashPosition;

    // Update is called once per frame
    void Update() {
        dashCooldownRemaining = Mathf.Clamp(dashCooldownRemaining - Time.deltaTime, 0.0f, Mathf.Infinity);
        isCharging = charge && !movementController.isDashing && dashCooldownRemaining == 0.0f;

        if (isCharging) {
            chargeRadius = Mathf.Clamp(chargeRadius + (dashMaxDistance / dashChargeTime) * Time.deltaTime, 0.0f, dashMaxDistance);
            Vector2 currentPosition = movementController.characterMover.characterPosition;
            Vector2 delta = Vector2.ClampMagnitude(desiredDashPosition - currentPosition, chargeRadius);
            var hit = movementController.characterMover.FirstObstacle(delta);
            if (hit) {
                delta = delta.normalized * hit.distance;
            }
            actualRadius = delta.magnitude;
            actualDashPosition = currentPosition + delta;
        } else {
            chargeRadius = 0.0f;
        }
    }

    public void TriggerDash() {
        if (actualRadius >= dashMinDistance) {
            movementController.DashToPoint(actualDashPosition);
        }
    }

    private void OnDrawGizmosSelected() {
        if(isCharging) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(movementController.characterMover.characterPosition, chargeRadius);
            Gizmos.DrawLine(movementController.characterMover.characterPosition, actualDashPosition);
        }
    }
}
