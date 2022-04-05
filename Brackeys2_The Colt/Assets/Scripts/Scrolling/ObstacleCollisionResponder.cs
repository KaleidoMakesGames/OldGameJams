using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObstacleCollisionResponder : MonoBehaviour
{
    public UnityEvent OnCollision;

    private void Update() {
        
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(enabled && collision.collider.GetComponentInParent<Obstacle>() != null) {
            OnCollision.Invoke();
        }
    }
}
