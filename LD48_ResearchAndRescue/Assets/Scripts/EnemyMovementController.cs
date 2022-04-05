using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Rigidbody rb;

    public Vector3? targetPosition;

    public float targetRange;

    public float movementSpeed;

    public float damageRecoilTime;

    public bool isRecoiling { get; private set; }

    private float _lastDamageTime;

    // Update is called once per frame
    void Update()
    {
        if(Time.time - _lastDamageTime >= damageRecoilTime && targetPosition.HasValue) {
            rb.isKinematic = true;
            agent.enabled = true;
            agent.destination = targetPosition.Value;
            agent.stoppingDistance = targetRange;
            agent.speed = movementSpeed;
            isRecoiling = false;
        } else {
            agent.enabled = false;
            rb.isKinematic = false;
            isRecoiling = true;
        }
    }

    public void TakeDamage() {
        _lastDamageTime = Time.time;
        Update();
    }
}
