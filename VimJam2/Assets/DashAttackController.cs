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
    public float maxDistance;
    public float chargeRate;
    public float chargeTime;
    public float cooldownTime;

    [Header("References")]
    public TopDownMovementController movementController;

    public enum State { IDLE, CHARGING, BURNOUT, COOLDOWN}
    [Header("State")]
    [ReadOnly] public State currentState;
    [ReadOnly] public float cooldownTimeRemaining;
    [ReadOnly] public float chargeTimeRemaining;
    [ReadOnly] public float currentChargeDistance;
    [ReadOnly] public Vector2 actualDashPosition;

    // Update is called once per frame
    void Update() {
        cooldownTimeRemaining = Mathf.Clamp(cooldownTimeRemaining - Time.deltaTime, 0.0f, Mathf.Infinity);
        chargeTimeRemaining = Mathf.Clamp(chargeTimeRemaining - Time.deltaTime, 0.0f, Mathf.Infinity);
        switch(currentState) {
            case State.IDLE:
                if(charge) {
                    currentState = State.CHARGING;
                    chargeTimeRemaining = chargeTime;
                }
                break;
            case State.CHARGING:
                currentChargeDistance = Mathf.Clamp(currentChargeDistance + chargeRate * Time.deltaTime, 0, maxDistance);
                Vector2 currentPosition = movementController.characterMover.characterPosition;
                Vector2 delta = (desiredDashPosition - currentPosition).normalized * currentChargeDistance;
                var hit = movementController.characterMover.FirstObstacle(delta, delta.magnitude);
                if (hit) {
                    delta = delta.normalized * hit.distance;
                }
                actualDashPosition = currentPosition + delta;

                if (chargeTimeRemaining == 0) {
                    currentState = State.BURNOUT;
                }
                if(!charge) {
                    currentState = State.IDLE;
                }
                break;
            case State.BURNOUT:
                if(!charge) {
                    currentState = State.IDLE;
                }
                break;
            case State.COOLDOWN:
                if(cooldownTimeRemaining == 0) {
                    currentState = State.IDLE;
                }
                break;
        }

        if(currentState != State.CHARGING) {
            currentChargeDistance = 0;
        }
    }

    public void TriggerDash() {
        if (currentState == State.CHARGING) {
            movementController.DashToPoint(actualDashPosition);
            currentState = State.COOLDOWN;
            cooldownTimeRemaining = cooldownTime;
        }
    }
}
