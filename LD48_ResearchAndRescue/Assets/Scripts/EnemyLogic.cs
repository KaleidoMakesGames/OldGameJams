using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    public float visionRange;
    public float attackRange;
    public Transform visionSpot;
    public AttackController attackController;
    public EnemyMovementController movementController;

    private PlayerMovementController _playerTarget;

    [System.Serializable]
    public enum State { Idle, Seeking};
    [ReadOnly] public State state;

    private void Awake() {
        state = State.Idle;
        _playerTarget = FindObjectOfType<PlayerMovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_playerTarget == null) {
            enabled = false;
            return;
        }
        if(state == State.Seeking) {
            attackController.fireTarget = _playerTarget.transform.position;
            if (CanSeePlayer() && !movementController.isRecoiling) {
                attackController.TryFire();
            }
            movementController.targetPosition = _playerTarget.transform.position;
            movementController.targetRange = attackRange;
        } else {
            if (CanSeePlayer() && DistanceToPlayer() <= visionRange) {
                state = State.Seeking;
            }
        }
    }

    private float DistanceToPlayer() {
        return Vector3.Distance(visionSpot.position, _playerTarget.transform.position);
    }

    private bool CanSeePlayer() {
        Vector3 start = visionSpot.position;
        Vector3 end = _playerTarget.transform.position;
        end.y = start.y;
        RaycastHit hit;
        if(Physics.Raycast(start, end-start, out hit, (end-start).magnitude, ~LayerMask.GetMask("Player", "Enemy"))) {
            return false;
        }
        return true;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRange);
        if (_playerTarget != null) {
            Vector3 start = visionSpot.position;
            Vector3 end = _playerTarget.transform.position;
            end.y = start.y;
            if (state == State.Idle) {
                Gizmos.color = Color.yellow;
            } else {
                Gizmos.color = Color.red;
            }
            Gizmos.DrawLine(start, end);
        }
    }
}
