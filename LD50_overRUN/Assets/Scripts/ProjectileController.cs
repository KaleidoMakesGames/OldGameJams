using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public Vector2 spawnPosition;
    public Transform homingTarget;
    public Vector2 positionTarget;
    public float projectileSpeed;

    public UnityEngine.Events.UnityEvent OnReachedTarget;
    public UnityEngine.Events.UnityEvent OnTargetLost;

    private bool hadTarget;

    private void Start() {
        transform.position = spawnPosition;
        hadTarget = homingTarget != null;
    }

    private void Update() {
        if(homingTarget != null) {
            positionTarget = homingTarget.position;
        }

        transform.position = Vector2.MoveTowards(transform.position, positionTarget, projectileSpeed * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, positionTarget - (Vector2)transform.position);

        if (hadTarget && homingTarget == null) {
            OnTargetLost.Invoke();
            hadTarget = false;
        }
        if (Vector2.Distance(transform.position, positionTarget) == 0) {
            OnReachedTarget.Invoke();
            Destroy(gameObject);
        }
    }
}
